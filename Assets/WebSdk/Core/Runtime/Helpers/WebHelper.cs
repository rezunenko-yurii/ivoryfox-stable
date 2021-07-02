using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSdk.Core.Runtime.Helpers
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
        
        public static Dictionary<string, string> DecodeQueryParameters(Uri uri)
        {
            if (uri == null) return null;
            if (uri.Query.Length == 0) return new Dictionary<string, string>();
            
            return uri.Query.TrimStart('?')
                .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(parts => parts[0],
                    parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? parts[1] : ""))
                .ToDictionary(grouping => grouping.Key,
                    grouping => string.Join(",", grouping));
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