using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace IvoryFoxPackagesHelper
{
    public static class IvoryFoxPackagesInstaller
    {
        private const string PackageName = "com.ivoryfox.package-manager";
    
        [MenuItem("IvoryFox/Packages/Install Package Manager")]
        public static void Install()
        {
            var request = Client.Add(PackageName);
 
            while (!request.IsCompleted)
            {
                System.Threading.Tasks.Task.Delay(100);
            }
 
            if (request.Status != StatusCode.Success)
            {
                Debug.LogError("Cannot import " + PackageName + ": " + request.Error.message);
            }
        }
    }
}
