using System.Collections.Generic;
using UnityEngine;

namespace WebSdk.Core.Runtime.WebCore
{
    public class DummyWebManager : IWebManager
    {
        public DummyWebManager()
        {
            Debug.Log($"--------- !!!!!!!!!!!! --------- DummyWebManager");
        }
        public void Init()
        {
            //throw new System.NotImplementedException();
        }

        public void InitModules(IWebFactory factory)
        {
            //throw new System.NotImplementedException();
        }

        public void LoadConfigs()
        {
            //throw new System.NotImplementedException();
        }

        public void InitConfigs(Dictionary<string, string> configs)
        {
            //throw new System.NotImplementedException();
        }
    }
}