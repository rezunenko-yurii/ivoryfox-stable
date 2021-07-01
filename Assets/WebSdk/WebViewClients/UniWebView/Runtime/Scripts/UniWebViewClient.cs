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

        private const string MerchantReference = "merchantReference";
        private readonly string[] _keyWords = {"apple-payment", "google-payment", "social"};
        
        private string _merchant = string.Empty;
        private string _startUrl = string.Empty;
        private global::UniWebView _webView;
        
        private void Awake()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(Awake)}");
            SetSettings();
        }
        
        public void Open(string url)
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(Open)} {url}");
            
            _startUrl = url;
            LoadUrl(url);
            _webView.Show();
        }

        private void LoadUrl(string url)
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(LoadUrl)} {url}");
            _webView.Load(url);
        }

        public void SetSettings()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(SetSettings)}");
            
            _webView = gameObject.AddComponent<global::UniWebView>();
            _webView.Frame = Screen.safeArea;
            
            AddListeners();
            HideToolbar();
        }
        
        private void PageFinished(global::UniWebView webview, int errorCode, string message)
        {
            Debug.Log($"PageFinished {_webView.Url}");
            
            SearchMerchantReference();
            
            if(IsUrlContainsAnyKey(_webView.Url)) ShowToolbar();
            else if(IsNavigationBarActive) HideToolbar();
        }
        
        private void SearchMerchantReference()
        {
            var query = WebHelper.DecodeQueryParameters(new Uri(_webView.Url));
            if (query.ContainsKey(MerchantReference))
            {
                query.TryGetValue(MerchantReference, out _merchant);
                if (!WebHelper.IsValidUrl(_merchant))
                {
                    _merchant = $"https://{_merchant}";
                }
            }
        }

        private void AddListeners()
        {
            _webView.OnPageFinished += PageFinished;
            _webView.OnOrientationChanged += OnOrientationChanged;
            
            backButton.onClick.AddListener(OnBackButtonClick);
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

        private float GetFrameHeightWithToolbar() => Screen.width > Screen.height ? 0.9f : 0.95f;
        
        private float CalculateToolbarHeight(float frameHeight) => Screen.safeArea.height * (1f - frameHeight);
        
        private void SetFrameSize(float x, float y, float w, float h) => _webView.Frame = new Rect(x, y, w, h);
        
        private void SetNavigationBarSize(float width, float height) => navigationBar.sizeDelta = new Vector2(width, height);

        private bool IsNavigationBarActive => navigationBar.gameObject.activeInHierarchy;
        
        private void SetToolbarState(bool isActive) => navigationBar.gameObject.SetActive(isActive);
        
        private void OnBackButtonClick() => LoadUrl(!string.IsNullOrEmpty(_merchant) ? _merchant : _startUrl);
        
        private bool IsUrlContainsAnyKey(string url) => _keyWords.Any(url.Contains);
        
        public IModulesHost Parent { get; set; }
    }
}
