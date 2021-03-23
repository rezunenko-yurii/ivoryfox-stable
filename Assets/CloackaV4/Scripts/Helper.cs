using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CloackaV4.Scripts
{
    public static class Helper
    {
        public static bool IsValidUrl(string uriName)
        {
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out var uriResult) 
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }
        public static string AttachParameters(string url, Dictionary<string,string> collection)
        {
            if (!IsValidUrl(url))
            {
                //RootPackage.Scripts.Root.logger.Send("Can`t add parameters to Html");
                return url;
            }
            
            var stringBuilder = new StringBuilder(url);
            string str = "?";

            if (url.Contains("?"))
            {
                str = "&";
            }

            if (collection != null)
            {
                foreach (KeyValuePair<string,string> pair in collection)
                {
                    string aName = pair.Key;
                    string aValue = pair.Value;
                
                    if (!url.Contains(aName))
                    {
                        stringBuilder.Append(str + aName + "=" + aValue);
                        str = "&";
                    }
                }
            }
            
            return stringBuilder.ToString();
        }
    
        public static  void LoadNextScene()
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
    
        public static int GetSdkInt() {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var version = new AndroidJavaClass("android.os.Build$VERSION")) {
                return version.GetStatic<int>("SDK_INT");
            }
#endif
            return 30;
        }
    
        public static string Encode(string json)
        {
            string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(base64Encoded));
            
            return base64Encoded;
        }

        public static int GetUpTime()
        {
            var jo = new AndroidJavaObject("android.os.SystemClock");
            int androidUptime = Mathf.RoundToInt(jo.CallStatic<long>("elapsedRealtime") * 0.001f);
            return androidUptime;
        }
    }
}