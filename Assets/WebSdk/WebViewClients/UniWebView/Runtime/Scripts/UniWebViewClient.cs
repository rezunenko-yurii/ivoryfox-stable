using UnityEngine;
using UnityEngine.UI;
using WebSdk.Core.Runtime.GlobalPart;
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
        private float _height = 1f;
        private bool _showBackButton = false;
        private void Awake()
        {
            Debug.Log($"UniWebViewClient Awake");
     
            _webView = gameObject.AddComponent<global::UniWebView>();
            UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Verbose;
            
            _webView.OnPageFinished += PageFinished;
            _webView.OnPageStarted += PageStart;

            _webView.Frame = Screen.safeArea;
            
            _webView.OnOrientationChanged += (view, orientation) =>
            {
                Debug.Log("UniwebView orientatin changed");
                //_webView.Frame = h == 1f ? Screen.safeArea : new Rect(Screen.safeArea.x, Screen.safeArea.y + Screen.safeArea.height * (1f - h), Screen.safeArea.width, Screen.safeArea.height * h);
                ShowBackButton(_showBackButton);
            };
        }
        

        void ShowBackButton(bool show)
        {
            if (show) _height = Screen.width > Screen.height ? 0.9f : 0.95f;
            else _height = 1f;
            
            float toolbarOffset = Screen.safeArea.height * (1f - _height);
            Debug.Log($"Univwebview ShowBackButton {toolbarOffset}");
            
            _webView.Frame = new Rect(Screen.safeArea.x, Screen.safeArea.y + toolbarOffset, Screen.safeArea.width, Screen.safeArea.height * _height);
            //navigationBar.offsetMax = new Vector2(0, toolbarOffset);
            navigationBar.sizeDelta = new Vector2(0, toolbarOffset);
            
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

        public IMediator Mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.Mediator = mediator;
        }
        
        private void PageFinished(global::UniWebView webview, int errorCode, string message)
        {
            //Debug.Log($"PageFinished: {_webView.Url}");
            
            if (string.IsNullOrEmpty(_merchant) && !_webView.Url.Equals(_startUrl))
            {
                if (_nextMerch)
                {
                    string[] tempArray = _webView.Url.Split("/"[0]);
                    _merchant = tempArray[2];
                    //Debug.Log($"merchant value: {merchant}");
                    _merchLook = true;
                    _webView.SetUserInteractionEnabled(true);
                }
                _nextMerch = true;
            }
        }
        
        private string _merchant = "";
        private bool _nextMerch = false;
        private bool _merchLook = false;
        private bool _checkToolbar = false;

        private void PageStart(global::UniWebView webview, string currentUrl)
        {
            
            if (_merchLook)
            {
                if (((!currentUrl.Contains("way") && !currentUrl.Contains("pay.") && !currentUrl.Contains(_merchant)) || currentUrl.Contains("social")))
                {
                    _showBackButton = true;
                }
                else if(_showBackButton)
                {
                    _showBackButton = false;
                }
                
                navigationBar.gameObject.SetActive(_showBackButton);
                ShowBackButton(_showBackButton);
            }
        }

        public IModulesHost Parent { get; set; }
    }
}
