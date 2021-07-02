using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.Global
{
    public interface IModulesHandler
    {
        event Action Completed; 
        void PrepareForWork();
        void ResolveDependencies(ModulesOwner owner);
        void DoWork();
    }
}