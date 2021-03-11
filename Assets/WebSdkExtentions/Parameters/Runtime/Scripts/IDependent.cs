﻿using System;
using System.Collections.Generic;

namespace IvoryFox.WebSDK.WebSdkCore.Parameters
{
    public interface IDependent
    {
        Type[] DependsOn();
        void SetDependencies(List<object> objects);
    }
}