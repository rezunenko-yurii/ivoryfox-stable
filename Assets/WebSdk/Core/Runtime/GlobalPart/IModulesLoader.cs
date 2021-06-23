using UnityEngine;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IModulesLoader
    {
        GameObject HostGameObject { get; }
        void InitModules(GameObject componentsHostObject, IModulesHost parent);
    }
}