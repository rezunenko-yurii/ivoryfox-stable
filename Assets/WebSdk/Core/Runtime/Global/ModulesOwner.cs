using System;
using System.Collections.Generic;

namespace WebSdk.Core.Runtime.Global
{
    public class ModulesOwner
    {
        private readonly Dictionary<Type, IModule> _modules = new Dictionary<Type, IModule>();
        
        public IModule Get(Type moduleType)
        {
            IModule m = null;
            
            if (_modules.ContainsKey(moduleType))
            {
                m = _modules[moduleType];
            }
            
            if (m == null)
            {
                foreach (var module in _modules)
                {
                    if (IsSameOrSubclass(moduleType, module.Key))
                    {
                        m = module.Value;
                        break;
                    }
                }
            }
            
            return m;
        }
        
        public bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase
                   || potentialBase.IsAssignableFrom(potentialDescendant);
        }
        
        public List<IModule> GetAll<T>()
        {
            var list = new List<IModule>();

            foreach (var module in _modules)
            {
                if (module.Value is T)
                {
                    list.Add(module.Value);
                }
            }

            return list;
        }
        
        
        public void Add(Type moduleType, IModule module)
        {
            _modules.Add(moduleType, module);
        }

        public void Add(params IModule[] modules)
        {
            foreach (var module in modules)
            {
                Add(module.GetType(), module);
            }
        }

        public void SatisfyRequirements(IModuleRequest module)
        {
            var types = module.GetRequiredModules();
            var requiredModules = new List<IModule>();

            foreach (var requirement in types)
            {
                if (_modules.TryGetValue(requirement, out var requiredModule))
                {
                    requiredModules.Add(requiredModule);
                }
            }
            
            module.SetRequiredModules(requiredModules);
        }
        
        public void SatisfyRequirements(params IModule[] modules)
        {
            foreach (var module in modules)
            {
                if (module is IModuleRequest request)
                {
                    SatisfyRequirements(request);
                }
            }
        }
    }
}