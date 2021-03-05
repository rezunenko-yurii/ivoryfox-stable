using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace BuildTools.Editor.Scripts
{
    public static class ManifestsChecker
    {
        public static void Inspect()
        {
            IEnumerable<string> paths = FindAll();
 
            foreach (string path in paths)
            {
                bool isChanged = false;
                 
                XmlDocument manifest = new XmlDocument();
                manifest.Load(path);
                 
                XmlElement manifestRoot = manifest.DocumentElement;
                 
                foreach (XmlNode node in manifestRoot.ChildNodes)
                {
                    if (node.Name == "application")
                    {
                        foreach (XmlAttribute attribute in node.Attributes)
                        {
                            if(attribute.Name.Equals("android:debuggable") && attribute.Value.Equals("true"))
                            {
                                attribute.Value = "false";
                                isChanged = true;
                                Debug.Log($"FIXED Debuggable in {path}");
                            }
                        }
                    }
                }
 
                if (isChanged)
                {
                    manifest.Save(path);
                }
            }
        }
         
        private static List<string> FindAll()
        {
            var guids = AssetDatabase.FindAssets("manifest");
            List<string> paths = new List<string>();
             
            foreach (string guid in guids)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guid);
                if (filePath.EndsWith(".xml"))
                {
                    paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                }
            }
             
            return paths;
        }
    }
}