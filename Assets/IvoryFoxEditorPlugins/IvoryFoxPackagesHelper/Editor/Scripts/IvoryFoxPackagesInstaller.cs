using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace IvoryFoxPackagesHelper.Editor.Scripts
{
    public static class IvoryFoxPackagesInstaller
    {
        private const string PackageName = "https://github.com/rezunenko-yurii/ivoryfox-stable.git?path=Assets/IvoryFoxEditorPlugins/IvoryFoxPackages";

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