using System.Collections.Generic;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.ConfigLoader
{
    public static class ConfigLoaderHelper
    {
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
    }
}