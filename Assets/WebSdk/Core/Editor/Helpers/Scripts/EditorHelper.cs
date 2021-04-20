using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WebSdk.Core.Editor.Helpers.Scripts
{
    public static class EditorHelper
    {
        public static string GetAssetPath(string name)
        {
#if UNITY_EDITOR
            string[] optionGuids = AssetDatabase.FindAssets(name);

            IList<string> optionPaths = new List<string>();
			
            foreach (string guid in optionGuids)
            {
                string optionPath = AssetDatabase.GUIDToAssetPath(guid);
                if (optionPath.EndsWith(name + ".unity"))
                {
                    optionPaths.Add(optionPath);
                }
            }

            if (optionPaths.Count == 0) return null;
            if (optionPaths.Count == 1) return optionPaths[0];
			
            Debug.LogError("Multiple " + name + " assets found! Aborting");  
#endif
            return null;
        }
    }
}