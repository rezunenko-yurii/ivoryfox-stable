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
            
            Debug.Log($"UniWebViewClient _screenHelper is {_screenHelper}");
        }

        private void ChangeOrientation(global::UniWebView webview, ScreenOrientation orientation)
        { 
            //_webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
            Debug.Log($"Uniwebview orientationChanged - {orientation}");
            
            _screenHelper.RecalculateSafeArea();
            //_webView.Frame = _screenHelper.GetMainRectTransform.rect;
            
            Debug.Log($"custom safeArea {_webView.Frame.center} {_webView.Frame.x} {_webView.Frame.y} {_webView.Frame.height} {_webView.Frame.width}");
            Debug.Log($"unity safeArea {Screen.safeArea.center} {Screen.safeArea.x} {Screen.safeArea.y} {Screen.safeArea.height} {Screen.safeArea.width}");
            
            _webView.Frame = Screen.safeArea;
        }
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
            Debug.Log($"UniWebViewClient SetSettings");
            
            _webView.SetContentInsetAdjustmentBehavior(UniWebViewContentInsetAdjustmentBehavior.Always);
            _webView.SetBackButtonEnabled(true);
            
            //_webView.Frame = new Rect(0, 0, Screen.safeArea.width, Screen.safeArea.height);
            Debug.Log($"UniWebViewClient _screenHelper.GetMainRectTransform is {_screenHelper.GetMainRectTransform is null}");
            Debug.Log($"UniWebViewClient _screenHelper.GetMainRectTransform.rect is {_screenHelper.GetMainRectTransform.rect}");
            Debug.Log($"UniWebViewClient Screen.safeArea is {Screen.safeArea}");
            _webView.Frame = Screen.safeArea;
            
            _webView.OnOrientationChanged += ChangeOrientation;
            _webView.OnShouldClose += OnShouldClose;
            
            _webView.SetToolbarDoneButtonText("");
            _webView.SetToolbarGoBackButtonText("Назад");
            _webView.SetToolbarGoForwardButtonText("");

            _webView.OnPageStarted += OnPageStarted;

            //Screen.orientation = ScreenOrientation.AutoRotation;
            //Screen.autorotateToPortrait = true;
        }

        private void OnPageStarted(global::UniWebView webview, string url)
        {
            Debug.Log($"WebView {url}");
            
            if (url.Contains("merchant"))
            {
                _webView.SetShowToolbar(true, true, true, true);
            }
            else
            {
                _webView.SetShowToolbar(false);
            }
        }

        public IMediator mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
