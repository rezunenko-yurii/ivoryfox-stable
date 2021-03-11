﻿using System;

namespace GlobalBlock.Interfaces
{
    public interface IUrlLoader: IModule, IConfigConsumer, IMediatorComponent
    {
        event Action<string> OnFailure;
        event Action<string> OnSuccess;
        void Start();
        string GetUrl();
        void RemoveListeners();
        void OnGetResponse(string response);
    }
}
