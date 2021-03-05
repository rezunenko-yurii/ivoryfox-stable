using UnityEditor;

namespace PluginsResolver.Editor.Scripts
{
    [InitializeOnLoad]
    public static class UnityPackagesLoader
    {
        static UnityPackagesLoader()
        {
            //EditorApplication.projectWindowChanged += OnProjectChanged;
        }

        private static void OnProjectChanged()
        {
            CheckPackagesToInstall();
        }
    
        private static void CheckPackagesToInstall()
        {
            if(EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += DownloadAndInstall;
                return;
            }

            DownloadAndInstall();
        }

        [MenuItem("IvoryFox/Download packages")]
        private static void DownloadAndInstall()
        {
            LocalPackagesInstaller.TryToInstall();
            UnityRegistryPackagesDownloader.Download();
        }
    }
}
