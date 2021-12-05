using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace WebSdk.Core.Editor.Helpers.Scripts
{
    public static class IOSPostBuildStep {
        // Set the IDFA request description:
        const string k_TrackingDescription = "Your data will be used to provide you a better and personalized experience.";

        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode) {
        
            Debug.Log("-------------------- IOS 14.5 ATT Post Process Build");
        
            if (buildTarget == BuildTarget.iOS) {
                AddPListValues(pathToXcode);
            }
        }

        // Implement a function to read and write values to the plist file:
        static void AddPListValues(string pathToXcode) {
/*#if UNITY_IOS
            // Retrieve the plist file from the Xcode project directory:
            string plistPath = pathToXcode + "/Info.plist";
            PlistDocument plistObj = new PlistDocument();


            // Read the values from the plist file:
            plistObj.ReadFromString(File.ReadAllText(plistPath));

            // Set values from the root object:
            PlistElementDict plistRoot = plistObj.root;

            // Set the description key-value in the plist:
            plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);

            // Save changes to the plist:
            File.WriteAllText(plistPath, plistObj.WriteToString());
            
            Debug.Log("-------------------- ATT Post Process Build Complete");
#endif*/
        }
    }
}