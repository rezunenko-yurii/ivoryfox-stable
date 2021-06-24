using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace WebSdk.Core.Runtime.WebCore.Url
{
    public sealed class UrlResponseReader
    {
        public string GetUrl(string value)
        {
            string url = string.Empty;
            ResponseFormat responseFormat = GetResponseType(value);

            if (responseFormat == ResponseFormat.Json) url = GetUrlFromJson(value);
            //else if (responseFormat == ResponseFormat.Html) url = GetUrlFromHtml(value);
            else if (responseFormat == ResponseFormat.Text) url = GetUrlFromText(value);

            return url;
        }
        
        public bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}");
        }
        
        public bool IsXml(string input)
        {
            input = input.Trim();
            return input.StartsWith("[") && input.EndsWith("]");
        }
        
        public bool IsHtml(string checkString)
        {
            return Regex.IsMatch(checkString, "<(.|\n)*?>");
        }
        
        public ResponseFormat GetResponseType(string t)
        {
            if (IsJson(t)) return ResponseFormat.Json;
            else if (IsHtml(t)) return ResponseFormat.Html;
            else if (IsXml(t)) return ResponseFormat.Xml;
            else return ResponseFormat.Text;
        }

        public string GetUrlFromHtml(string value)
        {
            throw new NotImplementedException();
        }

        public string GetUrlFromJson(string value)
        {
            throw new NotImplementedException();
        }

        public string GetUrlFromText(string value)
        {
            return UnityWebRequest.UnEscapeURL(value);
        }
    }
    
    public enum ResponseFormat { Text, Json, Xml, Html }
}