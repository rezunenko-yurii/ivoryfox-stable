using System;
using System.Collections.Generic;

namespace WebSdk.Core.Runtime.Global
{
    public interface IModuleRequest
    {
        List<Type> GetRequiredModules();
        void SetRequiredModules(List<IModule> modules);
    }
}