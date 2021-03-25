using System;
using Newtonsoft.Json.Linq;
using Unity.RemoteConfig.Editor;
using UnityEditor;
using UnityEngine;

namespace RemoteConfigHelper.Scripts
{
    public class RemoteConfigHelperWindow : EditorWindow
    {
        [MenuItem("IvoryFox/Remote Config Helper")]
        public static void ShowWindow() => GetWindow<RemoteConfigHelperWindow>("Remote Config Helper");

        private EnvironmentData _data;
        private RemoteConfigModel _model;
        private void OnGUI()
        {
            if (string.IsNullOrEmpty(Application.cloudProjectId))
            {
                EditorGUILayout.LabelField(label:"Error", label2:"To use remote config, you should first create cloud project id");
                return;
            }
        
            EditorGUILayout.Space(20);
        
            _model = (RemoteConfigModel) EditorGUILayout.ObjectField("Remote Config Model",_model, typeof(RemoteConfigModel),false);
            if (_model is null)
            {
                EditorGUILayout.Space(20);
                EditorGUILayout.LabelField(label:"Error", label2:"Chose RemoteConfigModel");
                return;
            }
        
            EditorGUILayout.Space(20);
        
            if (GUILayout.Button("Create")) SetEnvironment();
        }

        private void SetEnvironment()
        {
            _data = new EnvironmentData();
        
            RemoteConfigWebApiClient.CreateEnvironment(Application.cloudProjectId, _model.environmentName, OnException);
            RemoteConfigWebApiClient.environmentCreated += OnEnvironmentCreated;
        }
        private void OnEnvironmentCreated(string responce)
        {
            RemoteConfigWebApiClient.environmentCreated -= OnEnvironmentCreated;
            _data.environmentId = responce;
        
            RemoteConfigWebApiClient.SetDefaultEnvironment(Application.cloudProjectId, _data.environmentId);
        
            JArray ja = new JArray();
            foreach (var configItem in _model.configItems)
            {
                ja.Add(new JObject{["key"] = configItem.name, ["value"] = configItem.value, ["type"] = configItem.type});
            }

            RemoteConfigWebApiClient.postConfigRequestFinished += OnConfigPosted;
            RemoteConfigWebApiClient.PostConfig(Application.cloudProjectId, _data.environmentId, ja);
        }
        private void OnConfigPosted(string configId)
        {
            RemoteConfigWebApiClient.postConfigRequestFinished -= OnConfigPosted;
            _data.configId = configId;

            PostRules();
        }
        private void PostRules()
        {
            foreach (var rule in _model.rules)
            {
                var ja = new JArray();

                foreach (var configItem in rule.configItems)
                {
                    ja.Add(new JObject{["key"] = configItem.name, ["values"] = new JArray(configItem.value), ["type"] = configItem.type});
                }

                var newRule = new JObject
                {
                    ["projectId"] = Application.cloudProjectId,
                    ["environmentId"] = _data.environmentId,
                    ["configId"] = _data.configId,
                    ["id"] = Guid.NewGuid().ToString(),
                    ["name"] = rule.name,
                    ["enabled"] = rule.enabled,
                    ["condition"] = rule.conditions,
                    ["rolloutPercentage"] = rule.rolloutPercentage,
                    ["startDate"] = "",
                    ["endDate"] = "",
                    ["type"] = "segmentation",
                    ["value"] = ja
                };
            
                RemoteConfigWebApiClient.PostAddRule(Application.cloudProjectId, _data.environmentId, _data.configId, newRule, OnException);
            }
        
            Debug.Log("RemoteConfigHelper Done");
        }
        private void OnException(Exception obj) => Debug.Log(obj.Message);
    }
}
