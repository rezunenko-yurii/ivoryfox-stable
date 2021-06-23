using System;
using ImaginationOverflow.AndroidInstallReferrer;
using ImaginationOverflow.AndroidInstallReferrer.JavaInterop;
using UnityEngine;
using UnityEngine.Networking;

namespace CloackaV4.Scripts
{
    public class Referrer
    {
        public string RefValue;
        
        public Referrer()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            InstallReferrerManager.Instance.ReferrerInformationCollected += AnalyzeReferrer;
            InstallReferrerManager.Instance.FetchInformationCollected();
#else
            Debug.Log("UNITY EDITOR || REFERRER IS ORGANIC");
            RefValue = "utm_source=google-play&utm_medium=organic";
#endif
        }
        
        private void AnalyzeReferrer(InstallReferrerInfo data)
        {
            Debug.Log($"In AnalyzeReferrer");
            
            InstallReferrerManager.Instance.ReferrerInformationCollected -= AnalyzeReferrer;

            if (data is null || data.IsException)
            {
                Debug.Log($"ERROR! REFERRER IS ORGANIC ||" +
                          $" error={data?.Error}" +
                          $" isException={data?.IsException}" +
                          $" installReferrer={data?.InstallReferrer}");
                
                RefValue = "organic";
            }
            else
            {
                var referrerInfo = data.InstallReferrer;
                var parameters = referrerInfo.Split('&');
            
                Debug.Log($"Referrer info {referrerInfo}");

                foreach (var item in parameters)
                {
                    var subStrings = item.Split('=');
                    var key = subStrings[0];
                    var value = subStrings[1];
                
                    Debug.Log($"Referrer key={key} value={value}");

                    if (key.Equals("utm_source"))
                    {
                        RefValue = value;
                        break;
                    }
                }
                
                if (string.IsNullOrEmpty(RefValue))
                {
                    RefValue = UnityWebRequest.UnEscapeURL(referrerInfo);
                } 
            }
        }
    }
}