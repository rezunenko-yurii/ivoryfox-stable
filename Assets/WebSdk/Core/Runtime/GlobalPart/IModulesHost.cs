using System;
using System.Collections.Generic;
using WebSdk.Core.Runtime.ConfigLoader;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IModulesHost : IModule
    {
        Dictionary<Type,IModule> Modules { get; set; }
        IModule GetModule(Type moduleType);
        public void AddModule(Type moduleType, IModule module);
    }
}