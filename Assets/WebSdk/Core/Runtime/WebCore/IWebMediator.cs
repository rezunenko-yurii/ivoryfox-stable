using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IWebMediator : IMediator
    {
        void Init(GameObject webGameObject);
    }
}