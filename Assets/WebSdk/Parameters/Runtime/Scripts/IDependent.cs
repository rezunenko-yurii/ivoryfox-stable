using System;
using System.Collections.Generic;

namespace WebSdk.Parameters.Runtime.Scripts
{
    public interface IDependent
    {
        Type[] DependsOn();
        void SetDependencies(List<object> objects);
    }
}