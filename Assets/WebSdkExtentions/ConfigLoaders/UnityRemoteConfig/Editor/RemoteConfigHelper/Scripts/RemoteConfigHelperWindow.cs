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

        private EnvironmentData data;
        private RemoteConfigModel model;
        private void OnGUI()
        {
            if (string.IsNullOrEmpty(Application.cloudProjectId))
            {
                EditorGUILayout.LabelField(label:"Error", label2:"To use remote config, you should first create cloud project id");
                return;
            }
        
            EditorGUILayout.Space(20);
        
            model = (RemoteConfigModel) EditorGUILayout.ObjectField("Remote Config Model",model, typeof(RemoteConfigModel),false);
            if (model is null)
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
            data = new EnvironmentData();
        
            RemoteConfigWebApiClient.CreateEnvironment(Application.cloudProjectId, model.environmentName, OnException);
            RemoteConfigWebApiClient.environmentCreated += OnEnvironmentCreated;
        }
        private void OnEnvironmentCreated(string responce)
        {
            RemoteConfigWebApiClient.environmentCreated -= OnEnvironmentCreated;
            data.environmentId = responce;
        
            RemoteConfigWebApiClient.SetDefaultEnvironment(Application.cloudProjectId, data.environmentId);
        
            JArray ja = new JArray();
            foreach (var configItem in model.configItems)
            {
                ja.Add(new JObject{["key"] = configItem.name, ["value"] = configItem.value, ["type"] = configItem.type});
            }

            RemoteConfigWebApiClient.postConfigRequestFinished += OnConfigPosted;
            RemoteConfigWebApiClient.PostConfig(Application.cloudProjectId, data.environmentId, ja);
        }
        private void OnConfigPosted(string configId)
        {
            RemoteConfigWebApiClient.postConfigRequestFinished -= OnConfigPosted;
            data.configId = configId;

            PostRules();
        }
        private void PostRules()
        {
            foreach (var rule in model.rules)
            {
                var ja = new JArray();

                foreach (var configItem in rule.configItems)
                {
                    ja.Add(new JObject{["key"] = configItem.name, ["values"] = new JArray(configItem.value), ["type"] = configItem.type});
                }

                var newRule = new JObject
                {
                    ["projectId"] = Application.cloudProjectId,
                    ["environmentId"] = data.environmentId,
                    ["configId"] = data.configId,
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
            
                RemoteConfigWebApiClient.PostAddRule(Application.cloudProjectId, data.environmentId, data.configId, newRule, OnException);
            }
        
            Debug.Log("RemoteConfigHelper Done");
        }
        private void OnException(Exception obj) => Debug.Log(obj.Message);
    }
}
