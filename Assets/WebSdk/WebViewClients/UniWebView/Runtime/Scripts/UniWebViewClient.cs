using System;
using SafeArea;
using UnityEngine;
using UnityEngine.UI;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.WebCore;

namespace WebSdk.WebViewClients.UniWebView.Runtime.Scripts
{
    public class UniWebViewClient : MonoBehaviour, IWebViewClient
    {
        private global::UniWebView _webView;
        [SerializeField] RectTransform navigationBar;
        [SerializeField] RectTransform webviewContainer;
        [SerializeField] Button backButton;

        private string _startUrl;
        //private SafeAreaAdjuster _safeAreaAdjuster;
        float h = 1f;
        private void Awake()
        {
            Debug.Log($"UniWebViewClient Awake");
     
            _webView = gameObject.AddComponent<global::UniWebView>();
            UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Verbose;
            
            _webView.OnPageFinished += PageFinished;
            _webView.OnPageStarted += PageStart;
            
            _webView.OnOrientationChanged += (view, orientation) =>
            {
                Debug.Log("UniwebView orientatin changed");
                _webView.Frame = h == 1f ? Screen.safeArea : new Rect(Screen.safeArea.x, Screen.safeArea.y + Screen.safeArea.height * (1f - h), Screen.safeArea.width, Screen.safeArea.height * h);
            };
        }
        
        void ShowBackButton(bool show)
        {
            h = show ? 0.95f : 1f;

            _webView.Frame = new Rect(Screen.safeArea.x, Screen.safeArea.y+ Screen.safeArea.height*(1f-h), Screen.safeArea.width, Screen.safeArea.height * h);
        }


        private void OnBackButtonClick() =>  _webView.Load(_startUrl);

        public void Open(string url)
        {
            _startUrl = url;
            backButton.onClick.AddListener(OnBackButtonClick);
            
            _webView.Load(url);
            _webView.Show();
        }

        public void SetSettings()
        {
            
        }

        public IMediator mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        private void PageFinished(global::UniWebView webview, int errorCode, string message)
        {
            //Debug.Log($"PageFinished: {_webView.Url}");
            
            if (string.IsNullOrEmpty(merchant) && !_webView.Url.Equals(_startUrl))
            {
                if (nextMerch)
                {
                    string[] tempArray = _webView.Url.Split("/"[0]);
                    merchant = tempArray[2];
                    //Debug.Log($"merchant value: {merchant}");
                    merchLook = true;
                    _webView.SetUserInteractionEnabled(true);
                }
                nextMerch = true;
            }
        }
        
        string merchant = "";
        bool nextMerch = false;
        bool merchLook = false;
        bool checkToolbar = false;

        private void PageStart(global::UniWebView webview, string currentUrl)
        {
            
            if (merchLook)
            {
                if (((!currentUrl.Contains("way") && !currentUrl.Contains("pay.") && !currentUrl.Contains(merchant)) || currentUrl.Contains("social"))) //&& !checkToolbar)
                {
                    navigationBar.gameObject.SetActive(true);
                    //webviewContainer.offsetMax = new Vector2(0, -100);//75

                    ShowBackButton(true);
                }
                else if(navigationBar.gameObject.activeInHierarchy)
                {
                    navigationBar.gameObject.SetActive(false);
                    ShowBackButton(false);
                    //webviewContainer.offsetMax = new Vector2(0, 0);//75
                }
            }
        }
    }
}
