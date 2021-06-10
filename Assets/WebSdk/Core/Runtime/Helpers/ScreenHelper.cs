using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    [ExecuteInEditMode]
    public class ScreenHelper : MonoBehaviour
    {
        public event Action OnRectChange;
        public event Action OnOrientationChanged;
        private RectTransform _mainRectTransform;
        public RectTransform GetMainRectTransform => _mainRectTransform;

        private DeviceOrientation _currentDeviceOrientation;
        private ScreenOrientation _currentScreenOrientation;
        private void Awake()
        {
            Debug.Log($"ScreenHelper Awake");
            _currentDeviceOrientation = Input.deviceOrientation;
        }

        private void Start()
        {
            _mainRectTransform = GetComponent<RectTransform>();
            
            RecalculateSafeArea();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Debug.Log($"ScreenHelper Update");
            Debug.Log($"SafeArea {Screen.safeArea}");
            if (Screen.orientation != _currentScreenOrientation)
#else
            if (Input.deviceOrientation != _currentDeviceOrientation)
#endif
            {
                Debug.Log($"ScreenHelper orientationChanged - {Input.deviceOrientation}");
                
                _currentDeviceOrientation = Input.deviceOrientation;
                _currentScreenOrientation = Screen.orientation;
                RecalculateSafeArea();
                OnOrientationChanged?.Invoke();
            }
        }
        
        void OnRectTransformDimensionsChange() 
        {
            Debug.Log($"ScreenHelper OnRectTransformDimensionsChange");
            
            OnRectChange?.Invoke();
        }

        public void RecalculateSafeArea()
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
        
            _mainRectTransform.anchorMin = anchorMin;
            _mainRectTransform.anchorMax = anchorMax;
            
            Debug.Log($"ScreenHelper RecalculateSafeArea // new rect {_mainRectTransform.rect.center} {_mainRectTransform.rect.x} {_mainRectTransform.rect.y} {_mainRectTransform.rect.height} {_mainRectTransform.rect.width}");
        }
    }
}
