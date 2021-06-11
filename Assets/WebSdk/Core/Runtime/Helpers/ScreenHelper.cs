using System;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    [ExecuteInEditMode]
    public class ScreenHelper : MonoBehaviour
    {
        public static event Action OnSafeRecalculated;
        private RectTransform _rectTransform;
        public Canvas _canvas;
        private bool isRecalculated = false;
        //public RectTransform GetRectTransform => _rectTransform;
        
        private void Awake()
        {
            Debug.Log($"ScreenHelper Awake");
            
            _rectTransform = GetComponent<RectTransform>();
            //_canvas = GetComponent<Canvas>();
            RefreshPanel(Screen.safeArea);
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
            Debug.Log($"Screen {Screen.width} {Screen.height}");
            Debug.Log($"Canvas {_canvas.scaleFactor}");
        
        
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
            
            isRecalculated = true;
            
        }

        private int i = 0;
        private void Update()
        {
            if (isRecalculated)
            {
                i = i + 1;

                if (i ==2 ) 
                {
                    Debug.Log($"--------------- ScreenHelper OnSafeRecalculated");
                    isRecalculated = false;
                    i=0;
                    
                    OnSafeRecalculated?.Invoke();
                    
                    
                }
            }
        }


        [ContextMenu("resize")]
        public void RecalculateSafeArea()
        {
            Debug.Log($"Safe area {Screen.safeArea}");
            Debug.Log($"Screen {Screen.width} {Screen.height}");
            Debug.Log($"currentResolution {Screen.currentResolution}");
            Debug.Log($"Canvas {_canvas.scaleFactor}");
        
        
            Vector2 anchorMin = Screen.safeArea.position;
            Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size;
        
            Debug.Log($"Anchors {anchorMin} {anchorMax}");

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
        
            Debug.Log($"After Anchors {anchorMin} {anchorMax}");

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
            
            Debug.Log($"--------------- ScreenHelper OnSafeRecalculated");
        }
    }
}
