using UnityEditor.iOS.Xcode;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#if UNITY_IOS
    //using Unity.Advertisement.IosSupport;
    using UnityEditor.iOS.Xcode;
#endif

namespace Adjust.Editor
{
    public class IosAttDescription
    {
        /// <summary>
        /// Description for IDFA request notification 
        /// [sets NSUserTrackingUsageDescription]
        /// </summary>
        const string TrackingDescription =
            "We collect Device ID data to analyze in-app purchases and make better propositions for you.";
        
        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                AddPListValues(pathToXcode);
            }
        }
        // Implement a function to read and write values to the plist file:
        static void AddPListValues(string pathToXcode) {
            // Retrieve the plist file from the Xcode project directory:
            string plistPath = pathToXcode + "/Info.plist";
            PlistDocument plistObj = new PlistDocument();


            // Read the values from the plist file:
            plistObj.ReadFromString(File.ReadAllText(plistPath));

            // Set values from the root object:
            PlistElementDict plistRoot = plistObj.root;

            // Set the description key-value in the plist:
            plistRoot.SetString("NSUserTrackingUsageDescription", TrackingDescription);

            // Save changes to the plist:
            File.WriteAllText(plistPath, plistObj.WriteToString());
        }
    }
}