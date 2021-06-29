using System;
using System.Collections.Generic;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public class ParametersWaiter
    {
        public event Action<string> Failed;
        public event Action Prepared;
        
        int _readyCounter;
        List<Parameter> _parameters;
        List<Parameter> _readyParams;

        public ParametersWaiter(List<Parameter> parameters)
        {
            _parameters = parameters;
            _readyParams = new List<Parameter>();
        }

        private void Remove(Parameter parameter, bool isPrepared = false)
        {
            if (!isPrepared) return;
            
            if (_readyParams.Contains(parameter))
            {
                _readyParams.Remove(parameter);
                _readyCounter--;
            }
        }

        public void CheckParam(Parameter param)
        {
            if (param.IsReady()) Add(param);
            else AddListeners(param);
        }
        private void Add(Parameter parameter, bool isPrepared = true)
        {
            if(!isPrepared) return;
            if (_readyParams.Contains(parameter)) return;
            
            _readyParams.Add(parameter);
            _readyCounter++;

            CheckIsAllPrepared();
        }

        private void CheckIsAllPrepared()
        {
            if (_readyCounter != _parameters.Count) return;

            Clear();
            OnPrepared();
        }

        private void OnPrepared()
        {
            Prepared?.Invoke();
            Prepared = null;
        }
        
        private void AddListeners(Parameter param)
        {
            param.Failed += OnFailed;
            param.Prepared += Add;
            param.Prepared += Remove;
        }
        
        private void RemoveListeners(Parameter attribute)
        {
            attribute.Failed -= OnFailed;
            attribute.Prepared -= Add;
            attribute.Prepared -= Remove;
        }

        private void OnFailed(Parameter parameter)
        {
            Clear();
            
            Failed?.Invoke($"Error!!! Can`t set value for attribute {parameter.GetAlias()}");
            Failed = null;
        }
        
        private void Clear()
        {
            _parameters.ForEach(RemoveListeners);
            _readyParams = null;
        }
    }
}