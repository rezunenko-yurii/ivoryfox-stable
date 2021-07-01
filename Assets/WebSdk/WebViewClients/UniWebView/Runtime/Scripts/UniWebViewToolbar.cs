using System;
using UnityEngine;
using UnityEngine.UI;

namespace WebSdk.WebViewClients.UniWebView.Runtime.Scripts
{
    public class UniWebViewToolbar
    {
        private RectTransform _toolbarTransform;
        private Button _backButton;
        
        public UniWebViewToolbar(RectTransform toolbarTransform, Button backButton)
        {
            _toolbarTransform = toolbarTransform;
            _backButton = backButton;
        }
        
        public void UpdateState()
        {
            if (IsActive()) Show();
            else Hide();
        }
        
        public void Show()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(Show)}");
            
            SetState(true);
            SetSize(0, GetHeight());
        }
            
        public void Hide()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(Hide)}");
                
            SetState(false);
            SetSize(0, 0f);
        }
        
        private float GetHeightCoef()
        {
            return Screen.width > Screen.height ? 0.1f : 0.05f;
        }

        public float GetHeight()
        {
            if (IsActive())
            {
                return Screen.safeArea.height*GetHeightCoef();
            }
            else
            {
                return 0;
            }
        }

        private void SetSize(float width, float height)
        {
            _toolbarTransform.sizeDelta = new Vector2(width, height);
        }
        
        public bool IsActive()
        {
            return _toolbarTransform.gameObject.activeInHierarchy;
        }
        
        private void SetState(bool isActive)
        {
            _toolbarTransform.gameObject.SetActive(isActive);
        }
    }
}