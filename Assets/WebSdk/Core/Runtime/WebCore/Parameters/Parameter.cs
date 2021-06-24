using System;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public abstract class Parameter : IModule
    {
        public Action<Parameter> onReady;
        public Action<Parameter> onUnReady;
        public Action<Parameter> onError;

        protected string parameterAlias = "alias";
        protected string Value;

        public virtual void Init(MonoBehaviour monoBehaviour){}
        public virtual bool IsReady() => !string.IsNullOrEmpty(Value);
        public virtual string GetAlias() => parameterAlias;
        public virtual string GetValue() => Value;
        public virtual void SetValue(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                Value = newValue;
                
                onReady?.Invoke(this);
            }
            else
            {
                onUnReady?.Invoke(this);
            }
        }
        
        public virtual void HandleOuterData(string newValue) { }
        public virtual void SetAlias(string alias)
        {
            parameterAlias = alias;
        }

        public virtual Parameter InitByModel(ParameterModel model)
        {
            parameterAlias = model.alias;
            HandleOuterData(model.value);

            return this;
        }

        public IModulesHost Parent { get; set; }
    }
}