using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.WebCore;

namespace WebSdk.Parameters.Runtime.Scripts
{
    public class ParametersManager : MonoBehaviour, IParamsManager
    {
        public event Action<string> OnError;
        public event Action OnComplete;

        public List<Parameter> parameters = new List<Parameter>();
        private List<Parameter> _readyParams = new List<Parameter>();

        private int _readyAttributesCounter = 0;
        private ParameterItems _parameterItems;

        public string ConfigName { get; } = "paramsConfig";
        public void SetConfig(string json) => _parameterItems = JsonUtility.FromJson<ParameterItems>(json);
        
        public void Init()
        {
            SerialiseParams(_parameterItems.items);
            
            foreach (Parameter param in parameters)
            {
                param.Parent = this;
                
                SetParamDependencies(param);
                param.Init(this);
                
                if (param.IsReady())
                {
                    CheckParamsReady(param);
                }
                else
                {
                    param.onError += StopParametersChecking;
                    param.onReady += CheckParamsReady;
                    param.onUnReady += RemoveFromReadyParams;
                }
            }
            
            if (parameters.Count == 0)
            {
                OnComplete?.Invoke();
                Mediator.Notify(this,"OnParamsLoaded");
            }
        }

        public Dictionary<string, string> GetParams() => ConvertToDictionary();

        private void SetParamDependencies(Parameter parameter)
        {
            IDependent dependent = parameter as IDependent;
            
            if(dependent is null) return;

            var types = dependent.DependsOn();
            var dependencies = new List<object>();

            foreach (var obj in parameters)
            {
                if (types.Contains(obj.GetType())) dependencies.Add(obj);
            }
            
            dependent.SetDependencies(dependencies);
        }
        
        #region ATTRIBUTES EVENTS
        private void StopParametersChecking(Parameter parameter)
        {
            ClearAttributesEvents();
            OnError?.Invoke($"Error!!! Can`t set value for attribute {parameter.GetAlias()}");
            Mediator.Notify(this,"Error");
            
            OnError = null;
        }
        private void CheckParamsReady(Parameter parameter)
        {
            if (_readyParams.Contains(parameter)) return;
            
            _readyParams.Add(parameter);
            _readyAttributesCounter++;

            if (_readyAttributesCounter != parameters.Count) return;
                
            ClearAttributesEvents();

            OnComplete?.Invoke();
            OnComplete = null;
            
            Mediator.Notify(this,"OnParamsLoaded");
        }
        private Dictionary<string, string> ConvertToDictionary()
        {
            var dict = new Dictionary<string, string>();
            parameters.ForEach(parameter => dict.Add(parameter.GetAlias(), parameter.GetValue()));
            return dict;
        }
        private void RemoveFromReadyParams(Parameter parameter)
        {
            if (_readyParams.Contains(parameter))
            {
                _readyParams.Remove(parameter);
                _readyAttributesCounter--;
            }
        }
        private void ClearAttributesEvents()
        {
            foreach (var attribute in parameters)
            {
                attribute.onError -= StopParametersChecking;
                attribute.onReady -= CheckParamsReady;
                attribute.onUnReady -= RemoveFromReadyParams;
            }

            _readyParams = null;
        }
        #endregion
        
        #region FIND ATTRIBUTES REGION
        
        private void SerialiseParams(List<ParameterModel> parameterModels)
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
                    parameters.Add(parameter);
                        
                    Debug.Log($"::{nameof(ParametersManager)}.{nameof(SerialiseParams)}:: Serialised {model.id} {attribute.Key}");
                });
            }
            else
            {
                Debug.Log($"::{nameof(ParametersManager)}.{nameof(SerialiseParams)}:: ------------------ CAN`T SERIALISE PARAMS MODELS / CAN`T FIND ATTRIBUTES WITH LIST IDS ----------");
            }
            
        }
        #endregion
        
        public IMediator Mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            Mediator = mediator;
        }

        public IModulesHost Parent { get; set; }
        public Dictionary<Type, IModule> Modules { get; set; }
        public IModule GetModule(Type moduleType)
        {
            return Parent.GetModule(moduleType);
        }

        public void AddModule(Type moduleType, IModule module)
        {
            Parent.AddModule(moduleType, module);
        }
    }
}