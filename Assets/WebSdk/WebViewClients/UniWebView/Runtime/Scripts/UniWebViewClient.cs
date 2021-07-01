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
        private UniWebViewToolbar _toolbar;
        
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

            _toolbar = new UniWebViewToolbar(navigationBar, backButton);
            
            AddListeners();
        }
        
        private void AddListeners()
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(AddListeners)}");
            
            _webView.OnPageFinished += PageFinished;
            _webView.OnOrientationChanged += OnOrientationChanged;
            
            backButton.onClick.AddListener(OnBackButtonClick);
        }
        
        private void PageFinished(global::UniWebView webview, int errorCode, string message)
        {
            Debug.Log($"PageFinished {_webView.Url}");
            
            SearchMerchantReference();

            if (IsUrlContainsAnyKey(_webView.Url))
            {
                _toolbar.Show();
            }
            else if (_toolbar.IsActive())
            {
                _toolbar.Hide();
            }
            
            UpdateScreen();
        }
        
        private bool IsUrlContainsAnyKey(string url)
        {
            return _keyWords.Any(url.Contains);
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
        
        private void OnOrientationChanged(global::UniWebView view, ScreenOrientation orientation)
        {
            Debug.Log($"{nameof(UniWebViewClient)} {nameof(OnOrientationChanged)}");
            _toolbar.UpdateState();
            
            UpdateScreen();
        }
        
        private void UpdateScreen()
        {
            var toolbarHeight = _toolbar.GetHeight();
            SetFrameSize(Screen.safeArea.x, Screen.safeArea.y + toolbarHeight, Screen.safeArea.width, Screen.safeArea.height - toolbarHeight);
        }
        
        private void SetFrameSize(float x, float y, float w, float h)
        {
            _webView.Frame = new Rect(x, y, w, h);
        }
        
        private void OnBackButtonClick()
        {
            LoadUrl(!string.IsNullOrEmpty(_merchant) ? _merchant : _startUrl);
        }
        
        public IModulesHost Parent { get; set; }
    }
}
