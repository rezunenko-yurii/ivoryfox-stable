using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebSdk.Core.Runtime.Global
{
    public abstract class ModulesHost : MonoBehaviour, IModule
    {
        public ModulesHost Parent { get; set; }
        public Dictionary<Type, IModule> Modules { get; protected set; } = new Dictionary<Type, IModule>();
        public virtual IModule GetModule(Type moduleType)
        {
            if (Modules.ContainsKey(moduleType)) return Modules[moduleType];
            else return null;
        }
        protected virtual void AddModule(Type moduleType, IModule module) => Parent.AddModule(moduleType, module);

        protected virtual void AddModules(params IModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Parent = this;
                AddModule(module.GetType(), module);
            }
        }

        protected virtual void SetParent(ModulesHost parent)
        {
            Parent = parent;
            Modules = Parent.Modules;
        }
    }
}