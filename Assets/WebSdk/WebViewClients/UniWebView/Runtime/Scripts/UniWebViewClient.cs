using UnityEngine;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.WebCore;

namespace WebSdk.WebViewClients.UniWebView.Runtime.Scripts
{
    public class UniWebViewClient : IWebViewClient
    {
        private global::UniWebView _webView;
        private bool _isEscape;
        
        private const string ClickAgainToExit = "click again to exit";
        private const int TimeBetweenDoubleClick = 2;

        private ScreenHelper _screenHelper;
    
        public UniWebViewClient()
        {
            _screenHelper = GlobalFacade.MonoBehaviour.gameObject.GetComponent<ScreenHelper>();
            _webView = GlobalFacade.MonoBehaviour.gameObject.AddComponent<global::UniWebView>();
        }

        private void ChangeOrientation(global::UniWebView webview, ScreenOrientation orientation)
        { 
            //_webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
            _webView.Frame = _screenHelper.GetMainRectTransform.rect;
        }
        private void DisableDoubleClick() => _isEscape = false;
    
        private bool OnShouldClose(global::UniWebView webview)
        {
            if (_isEscape)
            {                        
                Close();
                Application.Quit();
                return true;
            }
            else
            {
                _isEscape = true;
            
                if (!_webView.IsInvoking(nameof(DisableDoubleClick)))
                {
                    _webView.Invoke(nameof(DisableDoubleClick), TimeBetweenDoubleClick);
                }
            }

            return false;
        }
    
        private void Close() 
        {
            if(_webView.IsInvoking(nameof(DisableDoubleClick)))
            {
                _webView.CancelInvoke(nameof(DisableDoubleClick));
            }
        
            _webView.CleanCache();
            _webView = null;
        }

        public void Open(string url)
        {
            _webView.Load(url);
            _webView.Show();
        }

        public void SetSettings()
        {
            _webView.SetContentInsetAdjustmentBehavior(UniWebViewContentInsetAdjustmentBehavior.Always);
            //uniWV.SetImmersiveModeEnabled(true);
            _webView.SetBackButtonEnabled(true);
            
            //_webView.Frame = new Rect(0, 0, Screen.safeArea.width, Screen.safeArea.height);
            _webView.Frame = _screenHelper.GetMainRectTransform.rect;
            
            _webView.OnOrientationChanged += ChangeOrientation;
            _webView.OnShouldClose += OnShouldClose;
            
            _webView.SetShowToolbar(true, true, false, true);
            _webView.SetToolbarDoneButtonText("Exit");
            _webView.OnPageStarted += OnPageStarted;

            //Screen.orientation = ScreenOrientation.AutoRotation;
            //Screen.autorotateToPortrait = true;
        }

        private void OnPageStarted(global::UniWebView webview, string url)
        {
            Debug.Log($"WebView {url}");
            Debug.Log($"remember_me {(global::UniWebView.GetCookie(url, "remember_me"))}");
            Debug.Log($"social_id {(global::UniWebView.GetCookie(url, "social_id"))}");
            Debug.Log($"php session id {(global::UniWebView.GetCookie(url, "php session id"))}");
            Debug.Log($"php_session_id {(global::UniWebView.GetCookie(url, "php_session_id"))}");
        }

        public IMediator mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
