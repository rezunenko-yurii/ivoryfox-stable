using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
/*using CloudContentDelivery.Editor.Scripts;
using CloudContentDeliveryClient.Api;*/
using GlobalBlock.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace GlobalBlock.ConfigLoaders.CloudContentDelivery.Runtime.Scripts
{
    public class CloudContentLoader : IConfigsLoader
    {
        //private CloudContentSettings settings;
        private Dictionary<string, string> data;
        private List<string> configNames;
        private Action<Dictionary<string, string>> callback;
    
        public void Load(string configName, Action<Dictionary<string, string>> onComplete)
        {
            var list = new List<string>{configName};
            Load(list, onComplete);
        }

        public void Load(List<string> configNames, Action<Dictionary<string, string>> onComplete)
        {
            this.configNames = configNames;
            callback = onComplete;

            if (data is null) DownloadConfig();
            GetDataFromConfig();
        }

        private void GetDataFromConfig()
        {
            var dict = new Dictionary<string, string>();

            foreach (string configName in configNames)
            {
                if (!string.IsNullOrEmpty(configName))
                {
                    dict.Add(configName, data.FirstOrDefault(x => x.Key.Equals(configName)).Value);
                }
            }

            callback.Invoke(dict);
        }

        private void DownloadConfig()
        {
            /*string path = $"https://{Application.cloudProjectId}.client-api.unity3dusercontent.com/client_api/v1";
            settings = Resources.Load<CloudContentSettings>("CloudContentSettings");
            
            var c = new ContentApi(path);
            var e = new EntriesApi(path);
            var r = new ReleasesApi(path);

            var entry = e.GetEntryByPathPublic(settings.lastBucketData.bucketId, "config.json");
        
            if (entry != null)
            {
                string bid = settings.lastBucketData.bucketId;
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
                
                data = deserializeToDictionary(rawString);
            }*/
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
