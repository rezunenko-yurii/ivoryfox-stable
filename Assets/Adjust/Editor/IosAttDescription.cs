﻿using UnityEditor.iOS.Xcode;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#if UNITY_IOS
    using Unity.Advertisement.IosSupport;
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
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToXcode)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                AddPListValues(pathToXcode);
            }
        }
        static void AddPListValues(string pathToXcode)
        {
#if UNITY_IOS
            // Get Plist from Xcode project 
            string plistPath = pathToXcode + "/Info.plist";
            // Read in Plist 
            PlistDocument plistObj = new PlistDocument();
            plistObj.ReadFromString(File.ReadAllText(plistPath));
            // set values from the root obj
            PlistElementDict plistRoot = plistObj.root;
            // Set value in plist
            plistRoot.SetString("NSUserTrackingUsageDescription", TrackingDescription);
            // save
            File.WriteAllText(plistPath, plistObj.WriteToString());
#endif
        }
    }
}