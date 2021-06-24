using System;
using System.Collections.Generic;

namespace WebSdk.Core.Runtime.Global
{
    public interface IModulesHost : IModule
    {
        Dictionary<Type,IModule> Modules { get; set; }
        IModule GetModule(Type moduleType);
        public void AddModule(Type moduleType, IModule module);
    }
}