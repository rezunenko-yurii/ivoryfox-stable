using UnityEngine;
using UnityEngine.UI;

namespace WebSdk.WebViewClients.UniWebView.Runtime.Scripts
{
    public class UniWebViewToolbar
    {
        private RectTransform _transform;
        private Button _backButton;
        
        public UniWebViewToolbar(RectTransform transform, Button backButton)
        {
            _transform = transform;
            _backButton = backButton;
        }
        
        public void UpdateState()
        {
            if (IsActive()) Show();
            else Hide();
        }
        
        public void Show()
        {
            Debug.Log($"{nameof(UniWebViewToolbar)} {nameof(Show)}");
            
            SetState(true);
            SetSize(0, GetHeight());
        }
            
        public void Hide()
        {
            Debug.Log($"{nameof(UniWebViewToolbar)} {nameof(Hide)}");
                
            SetState(false);
            SetSize(0, 0f);
        }
        
        private float GetHeightСoefficient()
        {
            return Screen.width > Screen.height ? 0.1f : 0.05f;
        }

        public float GetHeight()
        {
            if (IsActive())
            {
                return Screen.safeArea.height*GetHeightСoefficient();
            }
            else
            {
                return 0;
            }
        }

        private void SetSize(float width, float height)
        {
            _transform.sizeDelta = new Vector2(width, height);
        }
        
        public bool IsActive()
        {
            return _transform.gameObject.activeInHierarchy;
        }

        public void SetActive(bool isActive)
        {
            _transform.gameObject.SetActive(isActive);
        }
        
        private void SetState(bool isActive)
        {
            _transform.gameObject.SetActive(isActive);
        }
    }
}