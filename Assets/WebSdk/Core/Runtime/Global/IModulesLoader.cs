using UnityEngine;

namespace WebSdk.Core.Runtime.Global
{
    public interface IModulesLoader
    {
        GameObject HostGameObject { get; }
        void InitModules(GameObject componentsHostObject, IModulesHost parent);
    }
}