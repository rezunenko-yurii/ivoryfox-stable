using UnityEngine;

namespace WebSdk.Core.Runtime.Helpers
{
    public class SafeAreaPanel : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            RefreshPanel(Screen.safeArea);
        }

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

    }
}
