using System;
using System.Collections.Generic;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public interface IDependent
    {
        Type[] DependsOn();
        void SetDependencies(List<object> objects);
    }
}