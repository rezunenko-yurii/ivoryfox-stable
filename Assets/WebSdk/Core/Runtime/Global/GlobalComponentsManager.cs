using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.Logger;

namespace WebSdk.Core.Runtime.Global
{
    public class GlobalComponentsManager : ModulesHost, IModulesManager
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
        
        public void InitModules(GameObject hostGameObject, ModulesHost parent)
        {
            HostGameObject = hostGameObject;
            
            Logger = HostGameObject.gameObject.GetComponent<ICustomLogger>() ?? HostGameObject.gameObject.AddComponent<DummyCustomLogger>();
            InternetChecker = HostGameObject.gameObject.GetComponent<IInternetChecker>() ?? HostGameObject.gameObject.AddComponent<DummyInternetChecker>();
            ConfigsLoader = HostGameObject.gameObject.GetComponent<IConfigsLoader>() ?? HostGameObject.gameObject.AddComponent<DummyConfigLoader>();
            
            SetParent(parent);
            AddModules(Logger, InternetChecker, ConfigsLoader);
        }
    }
}