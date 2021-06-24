using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    [Serializable]
    public class ParameterItems
    {
        [SerializeField]
        public List<ParameterModel> items = new List<ParameterModel>();
    }
    
    [Serializable]
    public class ParameterModel
    {
        public string id;
        public string alias;
        public string value;
    }
}