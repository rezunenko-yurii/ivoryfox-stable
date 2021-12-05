using TMPro;
using UnityEngine;

using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Tracking;
using WebSdk.Core.Runtime.WebCore;
using Debug = UnityEngine.Debug;

namespace WebSdk.Main.Runtime.Scripts
{
    public class WebSdkEntry : MonoBehaviour
    {
        private WebManager _webManager;
        private GlobalComponentsManager _globalManager;
        private TrackingManager _trackingManager;

        private ModulesOwner _modulesOwner;

        private void Awake()
        {
            Debug.Log("GlobalBlockUnity Awake");

            _modulesOwner = new ModulesOwner();

            _trackingManager = GetComponent<TrackingManager>();
            _globalManager = GetComponent<GlobalComponentsManager>();
            _webManager = GetComponent<WebManager>();
            
            _trackingManager.PrepareForWork();
            _globalManager.PrepareForWork();
            _webManager.PrepareForWork();
            
            _trackingManager.ResolveDependencies(_modulesOwner);
            _globalManager.ResolveDependencies(_modulesOwner);
            _webManager.ResolveDependencies(_modulesOwner);

            GameNavigation.SetWebBlockSettings();
            

            _trackingManager.Completed += OnTrackingManagerCompleted;
            _trackingManager.DoWork();
        }

        private void OnTrackingManagerCompleted()
        {
            _globalManager.Completed += _webManager.DoWork;
            _globalManager.DoWork();
        }

        private void OnDestroy()
        {
            Debug.Log($"WebSdkEntry OnDestroy");
        }
    }
}