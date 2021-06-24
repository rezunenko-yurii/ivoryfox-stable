using System;

namespace WebSdk.Core.Runtime.WebCore.Url
{
    [Serializable]
    public class UrlsConfig
    {
        public string url;
        public string server;

        public bool HasUrl => !string.IsNullOrEmpty(url);
        public bool HasServer => !string.IsNullOrEmpty(server);
    }
}