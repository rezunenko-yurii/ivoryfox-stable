using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.WebCore.WebView;

namespace WebSdk.WebViewClients.UniWebView.Runtime.Scripts
{
    public class UniWebViewClient : MonoBehaviour, IWebViewClient
    {
        [SerializeField] RectTransform navigationBar;
        [SerializeField] Button backButton;

        private global::UniWebView _webView;
        private string _startUrl;
        
        private void Awake()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(Awake)}");
            SetSettings();
        }

        public void SetSettings()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(SetSettings)}");
            
            _webView = gameObject.AddComponent<global::UniWebView>();
            _webView.Frame = Screen.safeArea;
            
            AddWebviewListeners();
            //UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Verbose;
            HideToolbar();
        }

        private void AddWebviewListeners()
        {
            _webView.OnPageFinished += PageFinished;
            //_webView.OnPageStarted += PageStart;
            _webView.OnOrientationChanged += OnOrientationChanged;
        }
        
        private void OnOrientationChanged(global::UniWebView view, ScreenOrientation orientation)
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(OnOrientationChanged)}");
            UpdateToolbarState();
        }

        private void UpdateToolbarState()
        {
            if (IsNavigationBarActive) ShowToolbar();
            else HideToolbar();
        }
        
        private float GetFrameHeightWithToolbar() => Screen.width > Screen.height ? 0.9f : 0.95f;
        private float CalculateToolbarHeight(float frameHeight) => Screen.safeArea.height * (1f - frameHeight);
        
        void ShowToolbar()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(ShowToolbar)}");
            
            var frameHeight = GetFrameHeightWithToolbar();
            var toolbarHeight = CalculateToolbarHeight(frameHeight);

            UpdateScreen(frameHeight, toolbarHeight);
            SetToolbarState(true);
        }
        
        private void HideToolbar()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(HideToolbar)}");
            
            UpdateScreen(1f, 0f);
            SetToolbarState(false);
        }

        private void UpdateScreen(float frameHeight, float toolbarHeight)
        {
            SetFrameSize(Screen.safeArea.x, Screen.safeArea.y + toolbarHeight, Screen.safeArea.width, Screen.safeArea.height * frameHeight);
            SetNavigationBarSize(0, toolbarHeight);
        }
        
        private void SetFrameSize(float x, float y, float w, float h) => _webView.Frame = new Rect(x, y, w, h);
        
        private void SetNavigationBarSize(float width, float height) => navigationBar.sizeDelta = new Vector2(width, height);

        private bool IsNavigationBarActive => navigationBar.gameObject.activeInHierarchy;
        private void SetToolbarState(bool isActive) => navigationBar.gameObject.SetActive(isActive);
        
        private void OnBackButtonClick() =>  _webView.Load(_startUrl);

        public void Open(string url)
        {
            backButton.onClick.AddListener(OnBackButtonClick);
            
            _startUrl = url;
            _webView.Load(url);
            _webView.Show();
        }
        
        private void PageFinished(global::UniWebView webview, int errorCode, string message)
        {
            Debug.Log($"PageFinished {_webView.Url}");
            
            var query = WebHelper.DecodeQueryParameters(new Uri(_webView.Url));
            if(query.ContainsKey("merchantReference")) query.TryGetValue("merchantReference", out _merchant);

            bool isContainsKey = _keyWords.Any(s=>_webView.Url.Contains(s));
            if(isContainsKey)
            {
                ShowToolbar();
            }
            else if(IsNavigationBarActive)
            {
                HideToolbar();
            }
        }
        
        private string _merchant = "";
        private bool _nextMerch;
        private bool _merchLook;

        private void PageStart(global::UniWebView webview, string currentUrl)
        {
            Debug.Log($"PageStart {currentUrl}");
            if (_merchLook)
            {
                if (currentUrl.Contains("pay.") || currentUrl.Contains("social"))
                {
                    ShowToolbar();
                }
                else HideToolbar();
            }
        }

        public IModulesHost Parent { get; set; }

        private readonly string[] _keyWords = {"apple-payment", "google-payment", "social"};
    }
}
