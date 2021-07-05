using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Debug = UnityEngine.Debug;

namespace ReportTools.Editor.Scripts.ApkParser
{
    public class ApkReader
    {
        //private string ApkSignerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Android\Sdk\build-tools\30.0.2");
        public ApkInfo Info { get; }

        private static int VER_ID = 0;
        private static int ICN_ID = 1;
        private static int LABEL_ID = 2;
        private string[] VER_ICN = new string[3];

        // Some possible tags and attributes
        private string[] TAGS = { "manifest", "application", "activity" };
        private string[] ATTRS = { "android:", "a:", "activity:", "_:" };

        private Dictionary<string, object> _entryList = new Dictionary<string, object>();
        private List<string> _tmpFiles = new List<string>();
        private string _path;
        private string _apkSignerPath;
        
        public ApkReader(byte[] manifest_xml, byte[] resources_arsx, string path = null, string apkSignerPath = null)
        {
            string manifestXml;
            var manifest = new ApkManifest();
            _path = path;
            _apkSignerPath = apkSignerPath;
            
            try
            {
                manifestXml = manifest.ReadManifestFileIntoXml(manifest_xml);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            var doc = new XmlDocument();
            doc.LoadXml(manifestXml);
            Info = ExtractInfo(doc, resources_arsx);
        }

        private string FuzzFindInDocument(XmlDocument doc, string tag, string attr)
        {
            foreach (var t in TAGS)
            {
                var nodelist = doc.GetElementsByTagName(t);
                for (var i = 0; i < nodelist.Count; i++)
                {
                    var element = nodelist.Item(i);

                    if (element == null || element.NodeType != XmlNodeType.Element) 
                        continue;
                    
                    var map = element.Attributes;

                    if (map == null) 
                        continue;
                    
                    for (var j = 0; j < map.Count; j++)
                    {
                        var element2 = map.Item(j);
                        
                        if (element2.Name.EndsWith(attr))
                            return element2.Value;
                    }
                }
            }
            
            return null;
        }


        private XmlDocument InitDoc(string xml)
        {
            var initDoc = new XmlDocument();
            initDoc.LoadXml(xml);
            initDoc.DocumentElement?.Normalize();
            return initDoc;
        }

        private static void ExtractPermissions(ApkInfo info, XmlDocument doc)
        {
            ExtractPermission(info, doc, "uses-permission", "name");
            ExtractPermission(info, doc, "permission-group", "name");
            ExtractPermission(info, doc, "service", "permission");
            ExtractPermission(info, doc, "provider", "permission");
            ExtractPermission(info, doc, "activity", "permission");
        }
        
        private static bool ReadBoolean(XmlDocument doc, string tag, string attribute)
        {
            var str = FindInDocument(doc, tag, attribute);
            bool ret;
            
            try
            {
                ret = Convert.ToBoolean(str);
            }
            catch
            {
                ret = false;
            }
            
            return ret;
        }
        
        private static void ExtractSupportScreens(ApkInfo info, XmlDocument doc)
        {
            info.supportSmallScreens = ReadBoolean(doc, "supports-screens", "android:smallScreens");
            info.supportNormalScreens = ReadBoolean(doc, "supports-screens", "android:normalScreens");
            info.supportLargeScreens = ReadBoolean(doc, "supports-screens", "android:largeScreens");

            if (info.supportSmallScreens || info.supportNormalScreens || info.supportLargeScreens)
                info.supportAnyDensity = false;
        }

        private ApkInfo ExtractInfo(XmlDocument manifestXml, byte[] resourcesArsx)
        {
            var info = new ApkInfo();
            VER_ICN[VER_ID] = "";
            VER_ICN[ICN_ID] = "";
            VER_ICN[LABEL_ID] = "";
            
            try
            {
                var doc = manifestXml;
                
                if (doc == null)
                    throw new Exception("Document initialize failed");
                
                info.resourcesFileName = "resources.arsx";
                info.resourcesFileBytes = resourcesArsx;
                // Fill up the permission field
                ExtractPermissions(info, doc);

                // Fill up some basic fields
                info.minSdkVersion = FindInDocument(doc, "uses-sdk", "minSdkVersion");
                info.targetSdkVersion = FindInDocument(doc, "uses-sdk", "targetSdkVersion");
                info.versionCode = FindInDocument(doc, "manifest", "versionCode");
                info.versionName = FindInDocument(doc, "manifest", "versionName");
                info.packageName = FindInDocument(doc, "manifest", "package");
                info.label = FindInDocument(doc, "application", "label");
                info.screenOrientation = FindInDocument(doc, "activity", "screenOrientation");
                info.adjustAvailable = FindInDocument(doc, "receiver", "name") == "com.adjust.sdk.AdjustReferrerReceiver";
                info.keysHash = GetKeysHash();
                
                if (info.label.StartsWith("@"))
                    VER_ICN[LABEL_ID] = info.label;
                else if (int.TryParse(info.label, out var labelId))
                    VER_ICN[LABEL_ID] = $"@{labelId:X4}";
                
                // Get the value of android:debuggable in the manifest
                // "0" = false and "-1" = true
                info.debuggable = FindInDocument(doc, "application", "debuggable") ?? "0";

                // Fill up the support screen field
                ExtractSupportScreens(info, doc);

                if (info.versionCode == null)
                    info.versionCode = FuzzFindInDocument(doc, "manifest", "versionCode");

                if (info.versionName == null)
                    info.versionName = FuzzFindInDocument(doc, "manifest", "versionName");
                else if (info.versionName.StartsWith("@"))
                    VER_ICN[VER_ID] = info.versionName;

                var id = FindInDocument(doc, "application", "android:icon") ??
                         FuzzFindInDocument(doc, "manifest", "icon");

                if (id == null)
                {
                    Debug.Log("icon resId Not Found!");
                    return info;
                }

                // Find real strings
                if (!info.hasIcon)
                {
                    if (id.StartsWith("@android:"))
                        VER_ICN[ICN_ID] = $"@{id.Substring("@android:".Length)}";
                    else
                        VER_ICN[ICN_ID] = $"@{Convert.ToInt32(id):X4}";

                    var resId = VER_ICN.Where(t => t.StartsWith("@")).ToList();

                    var finder = new ApkResourceFinder();
                    info.resStrings = finder.ProcessResourceTable(info.resourcesFileBytes, resId);

                    if (!VER_ICN[VER_ID].Equals(""))
                    {
                        List<string> versions = null;
                        
                        if (info.resStrings.ContainsKey(VER_ICN[VER_ID].ToUpper()))
                            versions = info.resStrings[VER_ICN[VER_ID].ToUpper()];
                        
                        if (versions != null)
                        {
                            if (versions.Count > 0)
                                info.versionName = versions[0];
                        }
                        else
                        {
                            throw new Exception($"VersionName Cant Find in resource with id {VER_ICN[VER_ID]}");
                        }
                    }

                    List<string> iconPaths = null;
                    
                    if (info.resStrings.ContainsKey(VER_ICN[ICN_ID].ToUpper()))
                        iconPaths = info.resStrings[VER_ICN[ICN_ID].ToUpper()];
                    
                    if (iconPaths != null && iconPaths.Count > 0)
                    {
                        info.iconFileNameToGet = new List<string>();
                        info.iconFileName = new List<string>();
                        
                        foreach (var iconFileName in iconPaths)
                        {
                            if (iconFileName != null)
                            {
                                if(iconFileName.Contains(@"/"))
                                {
                                    info.iconFileNameToGet.Add(iconFileName);
                                    info.iconFileName.Add(iconFileName);
                                    info.hasIcon = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception($"Icon Cant Find in resource with id {VER_ICN[ICN_ID]}");
                    }

                    if (!VER_ICN[LABEL_ID].Equals(""))
                    {
                        List<string> labels = null;
                        
                        if (info.resStrings.ContainsKey(VER_ICN[LABEL_ID]))
                            labels = info.resStrings[VER_ICN[LABEL_ID]];
                        
                        if (labels != null && labels.Count > 0) 
                            info.label = labels[0];
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return info;
        }
        
        private static void ExtractPermission(ApkInfo info, XmlDocument doc, string keyName, string attribName)
        {
            var usesPermissions = doc.GetElementsByTagName(keyName);

            for (var s = 0; s < usesPermissions.Count; s++)
            {
                var permissionNode = usesPermissions.Item(s);
                if (permissionNode != null && permissionNode.NodeType == XmlNodeType.Element)
                {
                    var node = permissionNode.Attributes.GetNamedItem(attribName);
                    if (node != null)
                        info.permissions.Add(node.Value);
                }
            }
        }
        
        private static string FindInDocument(XmlDocument doc, string keyName, string attribName)
        {
            var usesPermissions = doc.GetElementsByTagName(keyName);

            for (var s = 0; s < usesPermissions.Count; s++)
            {
                var permissionNode = usesPermissions.Item(s);
                
                if (permissionNode != null && permissionNode.NodeType == XmlNodeType.Element)
                {
                    var node = permissionNode.Attributes.GetNamedItem(attribName);
                    
                    if (node != null)
                        return node.Value;
                }
            }
            
            return null;
        }

        private List<string> GetKeysHash()
        {
            var s = new List<string>();
            var processFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
            var processArguments = $"/c cd {_apkSignerPath} && " +
                                   $"apksigner.bat verify --print-certs -v \"{_path}\"";
            /*var processArguments = $"/c cd {_apkSignerPath} && " +
                                   $"apksigner.bat verify --print-certs -v \"{_path}\"";*/
            
            var process = new Process
            {
                StartInfo =
                {
                    FileName = processFileName,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    Arguments = processArguments
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            process.Close();

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
                return s;
            }

            try
            {
                var splitString = output.Split('\n');
                var certificateHash_SHA1 = splitString.First(i => i.Contains("Signer #1 certificate SHA-1 digest:"));
                var publicKey_SHA1 = splitString.First(i => i.Contains("Signer #1 public key SHA-1 digest:"));
            
                s.Add(Regex.Replace(certificateHash_SHA1, @"\t|\n|\r", ""));
                s.Add(Regex.Replace(publicKey_SHA1, @"\t|\n|\r", ""));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }

            //return $"{certificateHash_SHA1}\n{publicKey_SHA1}";
            return s;
        }
    }
}