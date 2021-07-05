using System;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public abstract class Parameter : MonoBehaviour, IModule
    {
        public event Action<Parameter> Prepared;
        public event Action<Parameter> Failed;

        public string Alias { get; protected set; } = "alias";

        private string _value;
        public string Value
        {
            get => _value;
            protected set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _value = value;
                    Prepared?.Invoke(this);
                }
            }
        }

        public abstract void Init();

        public bool IsPrepared()
        {
            return !string.IsNullOrEmpty(_value);
        }
    }
}