using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportTools.Editor.Scripts.ApkParser
{
    public class ApkInfo
    {
        public static int FINE = 0;
        public static int NULL_VERSION_CODE = 1;
        public static int NULL_VERSION_NAME = 2;
        public static int NULL_PERMISSION = 3;
        public static int NULL_ICON = 4;
        public static int NULL_CERT_FILE = 5;
        public static int BAD_CERT = 6;
        public static int NULL_SF_FILE = 7;
        public static int BAD_SF = 8;
        public static int NULL_MANIFEST = 9;
        public static int NULL_RESOURCES = 10;
        public static int NULL_DEX = 13;
        public static int NULL_METAINFO = 14;
        public static int BAD_JAR = 11;
        public static int BAD_READ_INFO = 12;
        public static int NULL_FILE = 15;
        public static int HAS_REF = 16;

        public string label;
        public string versionName;
        public string versionCode;
        public string minSdkVersion;
        public string targetSdkVersion;
        public string packageName;
        public string debuggable;
        public string screenOrientation;
        public List<string> permissions;
        public List<string> iconFileName;
        public List<string> iconFileNameToGet;
        public List<string> iconHash;
        public List<string> keysHash;
        public string resourcesFileName;
        public byte[] resourcesFileBytes;
        public bool hasIcon;
        public bool supportSmallScreens;
        public bool supportNormalScreens;
        public bool supportLargeScreens;
        public bool supportAnyDensity;
        public bool adjustAvailable;
        public Dictionary<string, List<string>> resStrings;
        public Dictionary<string, string> layoutStrings;

        public static bool SupportSmallScreen(byte[] dpi)
        {
            return dpi[0] == 1;
        }

        public static bool SupportNormalScreen(byte[] dpi)
        {
            return dpi[1] == 1;
        }

        public static bool SupportLargeScreen(byte[] dpi)
        {
            return dpi[2] == 1;
        }

        public ApkInfo()
        {
            hasIcon = false;
            supportSmallScreens = false;
            supportNormalScreens = false;
            supportLargeScreens = false;
            supportAnyDensity = true;
            versionCode = null;
            versionName = null;
            iconFileName = null;
            iconFileNameToGet = null;
            permissions = new List<string>();
        }
        
        public bool IsDebuggable
        {
            get
            {
                if (debuggable == null) 
                    return false; // debugabble is not in the manifest
                if (debuggable.Equals("-1")) 
                    return true; // debuggable == true
                return false;
            }
        }

        private bool IsReference(IEnumerable<string> strs)
        {
            try
            {
                if (strs.Any(IsReference))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        private static bool IsReference(string str)
        {
            try
            {
                if (str != null && str.StartsWith("@"))
                {
                    int.Parse(str, System.Globalization.NumberStyles.HexNumber);
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }
    }
}