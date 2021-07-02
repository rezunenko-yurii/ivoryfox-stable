using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Helpers;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public class ParametersManager : MonoBehaviour, IModule, IConfigConsumer, IModulesHandler
    {
        [SerializeField] private GameObject parametersGameObject;
        
        public event Action<string> Failed;
        public event Action Completed;

        Parameter[] _parameters;
        ParameterModels _parameterModels;
        ParametersWaiter _parametersWaiter;

        public string ConfigName { get; } = "paramsConfig";
        public void SetConfig(string json) => _parameterModels = JsonUtility.FromJson<ParameterModels>(json);

        public void PrepareForWork()
        {
            _parameters = parametersGameObject.gameObject.GetComponents<Parameter>();
        }
        
        public void ResolveDependencies(ModulesOwner owner)
        {
            if (_parameters.Length > 0)
            {
                foreach (Parameter param in _parameters)
                {
                    if (param is IModule module)
                    {
                        owner.Add(module);
                    }

                    if (param is IModuleRequest moduleRequest)
                    {
                        owner.SatisfyRequirements(moduleRequest);
                    }
                }
            }
        }

        public void DoWork()
        {
            if (_parameters.Length == 0) Completed?.Invoke();
            else
            {
                _parametersWaiter = new ParametersWaiter(_parameters);
                _parametersWaiter.Prepared += Completed;
                _parametersWaiter.Failed += Failed;
                
                foreach (Parameter param in _parameters)
                {
                    SetParamDependencies(param);
                    param.Init();
                
                    _parametersWaiter.CheckParam(param);
                }
            }
        }
        
        /*public void Init()
        {
            //SerialiseParams(_parameterModels.items);
            
            if (_parameters.Length == 0) Completed?.Invoke();
            else
            {
                _parametersWaiter = new ParametersWaiter(_parameters);
                _parametersWaiter.Prepared += Completed;
                _parametersWaiter.Failed += Failed;
                
                foreach (Parameter param in _parameters)
                {

                    SetParamDependencies(param);
                    param.Init(this);
                
                    _parametersWaiter.CheckParam(param);
                }
            }
        }*/

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

            foreach (var parameter in _parameters)
            {
                dict.Add(parameter.GetAlias(), parameter.GetValue());
            }
            
            return dict;
        }
        /*void SerialiseParams(List<ParameterModel> parameterModels)
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
        }*/
    }
}