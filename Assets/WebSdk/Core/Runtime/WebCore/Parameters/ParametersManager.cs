using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Helpers;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public class ParametersManager : ModulesHost, IConfigConsumer
    {
        public event Action<string> Failed;
        public event Action Completed;

        List<Parameter> _parameters = new List<Parameter>();
        ParameterModels _parameterModels;
        ParametersWaiter _parametersWaiter;

        public string ConfigName { get; } = "paramsConfig";
        public void SetConfig(string json) => _parameterModels = JsonUtility.FromJson<ParameterModels>(json);
        
        public void Init()
        {
            SerialiseParams(_parameterModels.items);
            
            if (_parameters.Count == 0) Completed?.Invoke();
            else
            {
                _parametersWaiter = new ParametersWaiter(_parameters);
                _parametersWaiter.Prepared += Completed;
                _parametersWaiter.Failed += Failed;
                
                foreach (Parameter param in _parameters)
                {
                    param.Parent = this;
                
                    SetParamDependencies(param);
                    param.Init(this);
                
                    _parametersWaiter.CheckParam(param);
                }
            }
        }

        public Dictionary<string, string> GetParams() => ConvertToDictionary();

        void SetParamDependencies(Parameter parameter)
        {
            if(!(parameter is IDependent dependent)) return;

            var types = dependent.DependsOn();
            var dependencies = new List<object>();

            foreach (var obj in _parameters)
            {
                if(types.Contains(obj.GetType())) dependencies.Add(obj);
            }
            
            dependent.SetDependencies(dependencies);
        }
        Dictionary<string, string> ConvertToDictionary()
        {
            var dict = new Dictionary<string, string>();
            _parameters.ForEach(parameter => dict.Add(parameter.GetAlias(), parameter.GetValue()));
            return dict;
        }
        void SerialiseParams(List<ParameterModel> parameterModels)
        {
            Debug.Log(" In ParametersManager.SerialiseParams");
            
            Dictionary<Type, IdAttribute> typesWithAttribute =  ReflectionHelper.GetTypesWithAttribute<IdAttribute>();

            if (typesWithAttribute.Count > 0 && parameterModels.Count > 0)
            {
                parameterModels.ForEach((model) =>
                {
                    var attribute = typesWithAttribute.Single((a) => a.Value.IsEquals(model.id));
                    if(attribute.Key is null) return;
                
                    Parameter parameter = ReflectionHelper.CreateByType<Parameter>(attribute.Key).InitByModel(model);
                    _parameters.Add(parameter);
                        
                    Debug.Log($"::{nameof(ParametersManager)}.{nameof(SerialiseParams)}:: Serialised {model.id} {attribute.Key}");
                });
            }
            else
            {
                Debug.Log($"::{nameof(ParametersManager)}.{nameof(SerialiseParams)}:: ------------------ CAN`T SERIALISE PARAMS MODELS / CAN`T FIND ATTRIBUTES WITH LIST IDS ----------");
            }
        }
    }
}