using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WebSdk.Core.Runtime.Helpers
{
    public static class SceneHelper
    {
        public static bool HasScene(string sceneName)
        {
            var numScenes = SceneManager.sceneCountInBuildSettings;
            
            for (int i = 0; i < numScenes; i++)
            {
                string sName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (sName.Equals(sceneName))
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public static void TryLoadScene(string name)
        {
            if (HasScene(name))
            {
                Debug.Log($"Load scene {name}");
                SceneManager.LoadScene(name);
            }
            else
            {
                Debug.Log($"Cannot find scene {name}");
            }
        }
        public static void LoadNextScene()
        {
            Debug.Log("Helper.LoadNextScene");
            
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex + 1 <= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                Application.Quit();
            }
        }
        
        public static void LoadFirstScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}