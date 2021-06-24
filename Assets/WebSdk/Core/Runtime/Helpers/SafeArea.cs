using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    public class SafeArea : MonoBehaviour
    {
        public static event Action Recalculated;
    
        private Rect lastSafeArea;
        private RectTransform parentRectTransform;
        private (int width, int height) _screen;

        private void Start() 
        {
            parentRectTransform = this.GetComponentInParent<RectTransform>();
        }

        private void Update() 
        {
            if (lastSafeArea != Screen.safeArea && _screen.width != Screen.width && _screen.height != Screen.height) 
            {
                ApplySafeArea();
            }
        }

        public void ApplySafeArea() 
        {
            Debug.Log($"SafeAreaNew ApplySafeArea {Screen.height} {Screen.width} {Screen.safeArea}");
            Rect safeAreaRect = Screen.safeArea;

            float scaleRatio = parentRectTransform.rect.width / Screen.width;

            var left = safeAreaRect.xMin * scaleRatio;
            var right = -( Screen.width - safeAreaRect.xMax ) * scaleRatio;
            var top = -safeAreaRect.yMin * scaleRatio;
            var bottom = ( Screen.height - safeAreaRect.yMax ) * scaleRatio;

            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2( left, bottom );
            rectTransform.offsetMax = new Vector2( right, top );

            lastSafeArea = Screen.safeArea;
            _screen.width = Screen.width;
            _screen.height = Screen.height;
            
            Recalculated?.Invoke();
        }
    }
}
