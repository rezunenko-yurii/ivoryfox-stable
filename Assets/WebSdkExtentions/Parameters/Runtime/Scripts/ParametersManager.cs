using System;
using System.Collections.Generic;
using System.Linq;
using com.ivoryfox.websdk.parameters.Runtime;
using Global.Helpers.Runtime;
using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.WebPart;
using IvoryFox.WebSDK.WebSdkCore.Parameters;
using UnityEngine;
using WebBlock.ivoryfox.websdk.parameters.Runtime;
using WebSdkExtentions.Parameters.Runtime.Scripts;

namespace IvoryFox.WebSDK.Parameters
{
    public class ParametersManager : IParamsManager
    {
        //private InputableParameters userParamsInput;
        public event Action<Dictionary<string,string>> OnAllReady;
        public event Action<string> OnError;
        public event Action OnComplete;

        public List<Parameter> parameters = new List<Parameter>();
        private List<Parameter> readyParams = new List<Parameter>();

        private int readyAttributesCounter = 0;
        private ParameterItems parameterItems;

        public string ConfigName { get; } = "paramsConfig";
        public void SetConfig(string json) => parameterItems = JsonUtility.FromJson<ParameterItems>(json);
        
        public void Init()
        {
            /*parameterItems.items.ForEach(param =>
            {
                logger.Send($"::{nameof(ParametersManager)}.{nameof(SetConfig)}:: " 
                                      + $"ParameterModel To Init -> name = {param.alias}" 
                                      + $"/ id = {param.id}"
                                      + $"/ value = {param.value}");
            });*/
            
            SerialiseParams(parameterItems.items);
            
            foreach (Parameter param in parameters)
            {
                SetParamDependencies(param);
                param.Init(GlobalFacade.monoBehaviour);
                
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

            /*List<Parameter> inputables = parameters.Where(param => param is IInputableParameter).ToList();;
            if (inputables.Count > 0)
            {
                Object prefab = Resources.Load("ParametersUI", typeof(GameObject));
                GameObject ui = GameObject.Instantiate(prefab, monoBehaviour.transform.parent) as GameObject;
                userParamsInput = ui.GetComponent<InputableParameters>();
                userParamsInput.Init(inputables, this);
            }*/

            if (parameters.Count == 0)
            {
                OnComplete?.Invoke();
                mediator.Notify(this,"OnParamsLoaded");
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
            mediator.Notify(this,"Error");
            
            OnError = null;
        }
        private void CheckParamsReady(Parameter parameter)
        {
            if (readyParams.Contains(parameter)) return;
            
            //logger.Send($"::{nameof(ParametersManager)}.{nameof(CheckParamsReady)}:: Parameter {parameter.GetAlias()} is ready | value = {parameter.GetValue()}");
            
            readyParams.Add(parameter);
            readyAttributesCounter++;

            if (readyAttributesCounter != parameters.Count) return;
                
            ClearAttributesEvents();

            var dict = ConvertToDictionary();
                
            OnAllReady?.Invoke(dict);
            OnAllReady = null;
                
            OnComplete?.Invoke();
            OnComplete = null;
            
            mediator.Notify(this,"OnParamsLoaded");
        }
        private Dictionary<string, string> ConvertToDictionary()
        {
            var dict = new Dictionary<string, string>();
            parameters.ForEach(parameter => dict.Add(parameter.GetAlias(), parameter.GetValue()));
            return dict;
        }
        private void RemoveFromReadyParams(Parameter parameter)
        {
            if (readyParams.Contains(parameter))
            {
                //logger.Send($"::{nameof(ParametersManager)}.{nameof(RemoveFromReadyParams)}:: Attribute {parameter.GetAlias()} is UNready", "", LogType.Warning);
            
                readyParams.Remove(parameter);
                readyAttributesCounter--;
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

            readyParams = null;
        }
        #endregion
        
        #region FIND ATTRIBUTES REGION
        private void SerialiseParams(List<ParameterModel> parameterModels)
        {
            Dictionary<Type, IdAttribute> typesWithAttribute =  ReflectionHelper.GetTypesWithAttribute<IdAttribute>();

            parameterModels.ForEach((model) =>
            {
                var attribute = typesWithAttribute.Single((a) => a.Value.IsEquals(model.id));
                if(attribute.Key is null) return;
                
                Parameter parameter = ReflectionHelper.CreateByType<Parameter>(attribute.Key).InitByModel(model);
                parameters.Add(parameter);
                        
                Debug.Log($"::{nameof(ParametersManager)}.{nameof(SerialiseParams)}:: Serialised {model.id} {attribute.Key}");
            });
        }
        #endregion


        public IMediator mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}