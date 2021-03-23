using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace CloackaV4.Scripts
{
    public class UrlResponseReader
    {
        public virtual string GetUrl(string value)
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
        
        public virtual ResponseFormat GetResponseType(string t)
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
            return value;
        }

        public string GetUrlFromText(string value)
        {
            return UnityWebRequest.UnEscapeURL(value);
        }
    }
    
    public enum ResponseFormat { Text, Json, Xml, Html }
}