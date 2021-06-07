using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    public class ScreenHelper : MonoBehaviour
    {
        private RectTransform _mainRectTransform;
        void Awake()
        {
            SetSafeArea();
        }

        private void SetSafeArea()
        {
            _mainRectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
        
            _mainRectTransform.anchorMin = anchorMin;
            _mainRectTransform.anchorMax = anchorMax;
        }

        public RectTransform GetMainRectTransform => _mainRectTransform;
    }
}
