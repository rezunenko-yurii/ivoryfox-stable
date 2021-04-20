using System;
using System.Collections.Generic;

namespace WebSdkExtensions.Parameters.Runtime.Scripts
{
    public interface IDependent
    {
        Type[] DependsOn();
        void SetDependencies(List<object> objects);
    }
}