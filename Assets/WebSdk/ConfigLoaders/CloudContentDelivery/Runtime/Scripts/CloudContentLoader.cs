using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CloudContentDelivery.Runtime.Scripts;
using CloudContentDeliveryClient.Api;
using Newtonsoft.Json;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.ConfigLoaders.CloudContentDelivery.Runtime.Scripts
{
    public class CloudContentLoader : IConfigsLoader
    {
        private CloudContentSettings _settings;
        private Dictionary<string, string> _data;
        private List<string> _configNames;

        public event Action<Dictionary<string, string>> Completed;

        public void Load(string configName)
        {
            var list = new List<string>{configName};
            Load(list);
        }

        public void Load(List<string> configNames)
        {
            _configNames = configNames;

            if (_data is null) DownloadConfig();
            GetDataFromConfig();
        }

        private void GetDataFromConfig()
        {
            var dict = new Dictionary<string, string>();

            foreach (string configName in _configNames)
            {
                if (!string.IsNullOrEmpty(configName))
                {
                    dict.Add(configName, _data.FirstOrDefault(x => x.Key.Equals(configName)).Value);
                }
            }

            Completed?.Invoke(dict);
            Completed = null;
        }

        private void DownloadConfig()
        {
            string path = $"https://{Application.cloudProjectId}.client-api.unity3dusercontent.com/client_api/v1";
            _settings = Resources.Load<CloudContentSettings>("CloudContentSettings");
            
            var c = new ContentApi(path);
            var e = new EntriesApi(path);
            var r = new ReleasesApi(path);

            var entry = e.GetEntryByPathPublic(_settings.lastBucketData.bucketId, "config.json");
        
            if (entry != null)
            {
                string bid = _settings.lastBucketData.bucketId;
                var rid = r.GetReleaseByBadgePublic(bid, "latest");
                
                Stream stream = c.GetReleaseContentPublic(bid, rid.Releaseid.ToString(), entry.Entryid.ToString());
                
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                
                string rawString = Encoding.ASCII.GetString(ms.ToArray());
                
                var sb = new StringBuilder(rawString.Length);

                foreach (char i in rawString)
                {
                    if (i != '\n' && i != '\r' && i != '\t') sb.Append(i);
                }
                
                rawString = sb.ToString();
                rawString = Regex.Replace(rawString, @"\s+", "");
                
                _data = deserializeToDictionary(rawString);
            }
        }

        private Dictionary<string, string> deserializeToDictionary(string jo)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
            var values2 = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> d in values)
            {
                values2.Add(d.Key, d.Value.ToString());
            }
            return values2;
        }
    }
}
