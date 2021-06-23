using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IModulesManager : IConfigsHandler, IModulesHost, IModulesLoader
    {
    }
}