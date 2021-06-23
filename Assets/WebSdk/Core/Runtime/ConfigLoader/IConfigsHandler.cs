using System.Collections.Generic;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.ConfigLoader
{
    public interface IConfigsHandler
    {
        List<string> GetConfigIds();
        List<IModule> GetModulesForConfigs();
    }
}