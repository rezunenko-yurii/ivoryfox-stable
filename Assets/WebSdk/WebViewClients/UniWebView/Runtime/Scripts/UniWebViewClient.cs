using System;
using System.Linq;
using RestSharp.Contrib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.WebCore.WebView;

namespace WebSdk.WebViewClients.UniWebView.Runtime.Scripts
{
    public class UniWebViewClient : MonoBehaviour, IWebViewClient
    {
        [SerializeField] RectTransform navigationBar;
        [SerializeField] Button backButton;
        [SerializeField] private TextMeshProUGUI _textfield;

        private const string MerchantReference = "merchantReference";
        private readonly string[] _keyWords = {"apple-payment", "google-payment", "social", "api.twitter.com", 
            "accounts.google.com", "facebook.com", "vk.com", "ok.ru", "mail.ru", "yandex.ru",
            "yandex.ua", "3dsecure", "skbbank.ru", "wallet.piastrix.com", "visasecure"};
        
        private string _merchant = string.Empty;
        private string _startUrl = string.Empty;
        private global::UniWebView _webView;
        private UniWebViewToolbar _toolbar;

        private IInternetChecker _internetChecker;
        const string noInternetText = "No internet connection. \n Please turn on the internet";
        const string loadingText = "Loading...";
        
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
            
            //SearchMerchantReference();

            bool canShow = CanShow(_webView.Url);

            //if (IsUrlContainsAnyKey(_webView.Url))
            if (canShow)
            {
                _toolbar.Show();
            }
            else if (_toolbar.IsActive())
            {
                _toolbar.Hide();
            }
            
            UpdateScreen();
        }

        private bool CanShow(string url)
        {
            string m = "merchantReference";
            var u = new Uri(url);
            
            string queryString = u.Query;
            var queryDictionary = HttpUtility.ParseQueryString(queryString);
            
            if(queryDictionary[m] != null)
            {
                _merchant = queryDictionary[m];
            }
            else
            {
                _merchant = string.Empty;
            }

            bool canShow = true;

            if (MerchantReference.Equals(string.Empty))
            {
                Debug.Log($"{nameof(UniWebViewClient)} {nameof(CanShow)} Hide button " +
                          $"| reason = MerchantReference is empty");
                canShow = false;
            }
            else if(u.Host.Equals(MerchantReference))
            {
                Debug.Log($"{nameof(UniWebViewClient)} {nameof(CanShow)} Hide button " +
                          $"| reason = MerchantReference equals Host");
                canShow = false;
            }
            else if(u.Host.Contains("pay.") && u.AbsolutePath.Contains("app"))
            {
                Debug.Log($"{nameof(UniWebViewClient)} {nameof(CanShow)} Hide button " +
                          $"| reason = Host contains pay and path contains app");
                canShow = false;
            }
            else if(u.Host.Contains("hpp.") && u.AbsolutePath.Contains("app"))
            {
                Debug.Log($"{nameof(UniWebViewClient)} {nameof(CanShow)} Hide button " +
                          $"| reason = Host contains hpp and path contains app");
                canShow = false;
            }

            return canShow;
        }
        
        /*private bool IsUrlContainsAnyKey(string url)
        {
            return _keyWords.Any(url.Contains);
        }*/
        
        /*private void SearchMerchantReference()
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
        }*/
        
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
            LoadUrl(!string.IsNullOrEmpty(_merchant) ? $"https://{_merchant}" : _startUrl);
        }

        public event Action Completed;
        public void PrepareForWork()
        {
            _internetChecker.Checked += InternetCheckerOnChecked;
            _internetChecker.Check(-1);
        }

        private void InternetCheckerOnChecked(bool hasConnection)
        {
            _webView.Alpha = hasConnection ? 1 : 0; 
            _toolbar.SetVisible(hasConnection);
            _textfield.text = hasConnection ? loadingText : noInternetText;
        }

        public void ResolveDependencies(ModulesOwner owner)
        {
            _internetChecker = (IInternetChecker) owner.Get(typeof(IInternetChecker));
        }

        public void DoWork()
        {
            //throw new NotImplementedException();
        }
    }
}
