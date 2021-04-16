using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebSdkExtensions.Parameters.Runtime.Scripts
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