using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    public class ScreenHelper : MonoBehaviour
    {
        public event Action OnOrientationChanged;
        private RectTransform _mainRectTransform;
        public RectTransform GetMainRectTransform => _mainRectTransform;

        private DeviceOrientation _currentOrientation;
        private void Awake()
        {
            _currentOrientation = Input.deviceOrientation;
        }

        private void Start()
        {
            _mainRectTransform = GetComponent<RectTransform>();
            
            RecalculateSafeArea();
        }

        private void Update()
        {
            if (Input.deviceOrientation != _currentOrientation)
            {
                Debug.Log($"ScreenHelper orientationChanged - {Input.deviceOrientation}");
                
                _currentOrientation = Input.deviceOrientation;
                RecalculateSafeArea();
                OnOrientationChanged?.Invoke();
            }
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
