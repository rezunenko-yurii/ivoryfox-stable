using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    [ExecuteInEditMode]
    public class ScreenHelper : MonoBehaviour
    {
        

        private RectTransform _rectTransform;
        public RectTransform GetRectTransform => _rectTransform;

        //private DeviceOrientation _currentDeviceOrientation;
        //private ScreenOrientation _currentScreenOrientation;
        private void Awake()
        {
            Debug.Log($"ScreenHelper Awake");
            
            _rectTransform = GetComponent<RectTransform>();
            RefreshPanel(Screen.safeArea);
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            RecalculateSafeArea();
        }

        /*private void Update()
        {
            if (Screen.orientation != _currentScreenOrientation)
            {
                Debug.Log($"ScreenHelper orientationChanged - {Input.deviceOrientation}");
                
                _currentDeviceOrientation = Input.deviceOrientation;
                _currentScreenOrientation = Screen.orientation;
                
                RecalculateSafeArea();
                OnOrientationChanged?.Invoke();
            }
        }*/
        
        /*void OnRectTransformDimensionsChange() 
        {
            //Debug.Log($"ScreenHelper OnRectTransformDimensionsChange");
            
            OnRectChange?.Invoke();
        }*/
        
        private void OnEnable()
        {
            SafeAreaDetection.OnSafeAreaChanged += RefreshPanel;
        }

        private void OnDisable()
        {
            SafeAreaDetection.OnSafeAreaChanged -= RefreshPanel;
        }
        
        
        private void RefreshPanel(Rect safeArea)
        {
            Debug.Log($"Safe area {Screen.safeArea}");
            Debug.Log($"Screen {Screen.height} {Screen.width}");
        
        
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
        
            Debug.Log($"Anchors {anchorMin} {anchorMax}");

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
        
            Debug.Log($"After Anchors {anchorMin} {anchorMax}");

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }

        public void RecalculateSafeArea()
        {
            /*var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
        
            _mainRectTransform.anchorMin = anchorMin;
            _mainRectTransform.anchorMax = anchorMax;
            
            Debug.Log($"ScreenHelper RecalculateSafeArea // new rect {_mainRectTransform.rect.center} {_mainRectTransform.rect.x} {_mainRectTransform.rect.y} {_mainRectTransform.rect.height} {_mainRectTransform.rect.width}");*/
        }
    }
}
