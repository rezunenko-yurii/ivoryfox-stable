using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Helpers.Runtime
{
    public static class WebHelper
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
                        //RootPackage.Scripts.Root.logger.Send($"Add attribute {aName} = {aValue} to link ");
                        stringBuilder.Append(str + aName + "=" + aValue);
                        str = "&";
                    }
                }
            }
            
            return stringBuilder.ToString();
        }
        
        /*public static string GetUserAgentTemplate()
        {
            var pattern = @"[()/]";
            var operationSystem = SystemInfo.operatingSystem;
            var cleanString = Regex.Replace(operationSystem, pattern, "");
            var userAgentTemplate = $"Mozilla/5.0 (Linux; {cleanString}) " +
                                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                    "Version/4.0 Chrome/74.0.3729.157 Mobile Safari/537.36";

            return userAgentTemplate;
        }*/
    }
}