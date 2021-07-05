using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public class ParametersWaiter
    {
        public event Action<string> Failed;
        public event Action Prepared;
        
        private int _readyCounter;
        private Parameter[] _parameters;
        private List<Parameter> _readyParams = new List<Parameter>();

        public ParametersWaiter(Parameter[] parameters)
        {
            _parameters = parameters;
        }

        private void Remove(Parameter parameter, bool isPrepared = false)
        {
            Debug.Log($"{nameof(ParametersWaiter)} {nameof(Remove)} // parameter={parameter.Alias} isPrepared={isPrepared}");
            
            if (!isPrepared)
            {
                return;
            }
            
            if (_readyParams.Contains(parameter))
            {
                _readyParams.Remove(parameter);
                _readyCounter--;
            }
        }

        public void CheckParam(Parameter param)
        {
            if (param.IsPrepared())
            {
                AddToPrepared(param);
            }
            else
            {
                AddListeners(param);
            }
        }
        private void AddToPrepared(Parameter parameter)
        {
            Debug.Log($"{nameof(ParametersWaiter)} {nameof(AddToPrepared)} // parameter={parameter.Alias}");
            
            if (_readyParams.Contains(parameter))
            {
                return;
            }
            
            _readyParams.Add(parameter);
            _readyCounter++;

            CheckIsAllPrepared();
        }

        private void CheckIsAllPrepared()
        {
            if (_readyCounter != _parameters.Length)
            {
                return;
            }

            Clear();
            OnPrepared();
        }

        private void OnPrepared()
        {
            Debug.Log($"{nameof(ParametersWaiter)} {nameof(OnPrepared)} is prepared");
            
            Clear();
            
            Prepared?.Invoke();
            Prepared = null;
        }
        
        private void AddListeners(Parameter parameter)
        {
            parameter.Failed += OnFailed;
            parameter.Prepared += AddToPrepared;
        }
        
        private void RemoveListeners(Parameter parameter)
        {
            parameter.Failed -= OnFailed;
            parameter.Prepared -= AddToPrepared;
        }

        private void OnFailed(Parameter parameter)
        {
            Clear();
            
            Failed?.Invoke($"Error!!! Can`t set value for attribute {parameter.Alias}");
            Failed = null;
        }
        
        private void Clear()
        {
            foreach (var parameter in _parameters)
            {
                RemoveListeners(parameter);
            }
            
            _readyParams = null;
        }
    }
}