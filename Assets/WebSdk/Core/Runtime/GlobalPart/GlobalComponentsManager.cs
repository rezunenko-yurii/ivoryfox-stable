using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.Logger;
using WebSdk.Core.Runtime.WebCore;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public class GlobalComponentsManager : MonoBehaviour, IModulesManager
    {
        public GameObject HostGameObject { get; private set; }
        public ICustomLogger Logger { get; private set; }
        public IInternetChecker InternetChecker { get; private set; }
        public IConfigsLoader ConfigsLoader { get; private set; }

        public List<string> GetConfigIds()
        {
            return ConfigLoaderHelper.GetConsumableIds(Logger, InternetChecker, ConfigsLoader);
        }

        public List<IModule> GetModulesForConfigs()
        {
            return new List<IModule> {Logger, InternetChecker, ConfigsLoader};
        }
        
        public void InitModules(GameObject hostGameObject, IModulesHost parent)
        {
            Parent = parent;
            HostGameObject = hostGameObject;
            
            Logger = HostGameObject.gameObject.GetComponent<ICustomLogger>() ?? HostGameObject.gameObject.AddComponent<DummyCustomLogger>();
            InternetChecker = HostGameObject.gameObject.GetComponent<IInternetChecker>() ?? HostGameObject.gameObject.AddComponent<DummyInternetChecker>();
            ConfigsLoader = HostGameObject.gameObject.GetComponent<IConfigsLoader>() ?? HostGameObject.gameObject.AddComponent<DummyConfigLoader>();

            Logger.Parent = this;
            InternetChecker.Parent = this;
            ConfigsLoader.Parent = this;
            
            Modules = Parent.Modules;
            
            Modules.Add(Logger.GetType(), Logger);
            Modules.Add(InternetChecker.GetType(), InternetChecker);
            Modules.Add(ConfigsLoader.GetType(), ConfigsLoader);
        }

        public Dictionary<Type, IModule> Modules { get; set; }
        public IModulesHost Parent { get; set; }

        public IModule GetModule(Type moduleType)
        {
            return Parent.GetModule(moduleType);
        }

        public void AddModule(Type moduleType, IModule module)
        {
            Parent.AddModule(moduleType, module);
        }
    }
}