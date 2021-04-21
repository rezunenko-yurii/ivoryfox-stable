using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.Helpers.Scripts
{
    public static class Helper
    {
        private static AndroidJavaClass toastJavaClass;
        public const string ClassName = "com.until.unit.ToastMsg";

        public static void ShowToast(string msg)
        {
            if (toastJavaClass == null)
            {
                toastJavaClass = new AndroidJavaClass(ClassName);
            }
            
            toastJavaClass.CallStatic("show", msg);
        }
        
        public static string EncodeDecrypt(string str)
        {
            ushort secretKey = 0x0088;
            
            string newStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                var ch = str.ToArray();
            
                foreach (var c in ch)
                {
                    newStr += TopSecret(c, secretKey);
                }
            }
            //Root.logger.Send($"EncodeDecrypt from {str} -> {newStr}");
            return newStr;
        }
        
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
 
        private static char TopSecret(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey);
            return character;
        }
        
        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }
        
        public static List<string> GetConsumableIds(params IModule[] modules)
        {
            List<string> ids = new List<string>();
            
            foreach (var module in modules)
            {
                if (module is IConfigConsumer consumer)
                {
                    ids.Add(consumer.ConfigName);
                }
            }

            return ids;
        }
        
        public static void SetConfigsToConsumables(Dictionary<string,string> configs, params IModule[] modules)
        {
            foreach (var module in modules)
            {
                if (module is IConfigConsumer consumer)
                {
                    configs.TryGetValue(consumer.ConfigName, out string config);
                    consumer.SetConfig(config);
                }
            }
        }
        
        public static void Shuffle<T> (this System.Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
        
        public static void Shuffle<T> (this System.Random rng, List<T> list)
        {
            int n = list.Count;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
    }
}
