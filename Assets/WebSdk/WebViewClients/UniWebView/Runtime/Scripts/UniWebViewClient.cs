using UnityEngine;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.WebCore;

namespace WebSdk.WebViewClients.UniWebView.Runtime.Scripts
{
    public class UniWebViewClient : IWebViewClient
    {
        private global::UniWebView _webView;
        private bool _isEscape;
        
        private const string ClickAgainToExit = "click again to exit";
        private const int TimeBetweenDoubleClick = 2;
    
        public UniWebViewClient()
        {
            _webView = GlobalFacade.monoBehaviour.gameObject.AddComponent<global::UniWebView>();
        }

        private void ChangeOrientation(global::UniWebView webview, ScreenOrientation orientation) => _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        private void DisableDoubleClick()
        {
            _isEscape = false;
            Debug.Log($"Uniwebview: reset isEscape / {_isEscape}");
        }

        private bool OnShouldClose(global::UniWebView webview)
        {
#if !UNITY_IOS
            Debug.Log($"Uniwebview: OnShouldClose / {_isEscape}");
            if (_isEscape)
            {            
                Debug.Log($"Uniwebview: Close Application");
                Close();
                Application.Quit();
                return true;
            }
            else
            {
                Debug.Log($"Uniwebview: Next click will close app");
                _isEscape = true;
            
                if (!webview.IsInvoking(nameof(DisableDoubleClick)))
                {
                    Debug.Log($"Uniwebview: Invoke DisableDoubleClick");
                    webview.Invoke(nameof(DisableDoubleClick), TimeBetweenDoubleClick);
                }
                else
                {
                    Debug.Log($"Uniwebview: can`t Invoke DisableDoubleClick");
                }
            }
#endif
            
            Debug.Log($"Uniwebview: go out from OnShouldClose");
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
            _webView.SetBackButtonEnabled(true);
            _webView.Frame = new Rect(0, 0, Screen.safeArea.width, Screen.safeArea.height);
            _webView.OnOrientationChanged += ChangeOrientation;
            _webView.OnShouldClose += OnShouldClose;
            
            _webView.SetToolbarDoneButtonText("");
            _webView.SetToolbarGoBackButtonText("Назад");
            _webView.SetToolbarGoForwardButtonText("");

            _webView.OnPageStarted += OnPageStarted;

            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
        }

        private void OnPageStarted(global::UniWebView webview, string url)
        {
            Debug.Log($"WebView {url}");
            
            if (url.Contains("pay."))
            {
                _webView.SetShowToolbar(true, true, false, true);
            }
            else
            {
                _webView.SetShowToolbar(false);
            }
            
            
            /*Debug.Log($"remember_me {(global::UniWebView.GetCookie(url, "remember_me"))}");
            Debug.Log($"social_id {(global::UniWebView.GetCookie(url, "social_id"))}");
            Debug.Log($"php session id {(global::UniWebView.GetCookie(url, "php session id"))}");
            Debug.Log($"php_session_id {(global::UniWebView.GetCookie(url, "php_session_id"))}");*/
        }

        public IMediator mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
