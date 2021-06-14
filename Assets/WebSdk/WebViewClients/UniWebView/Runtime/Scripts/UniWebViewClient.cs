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
        [SerializeField] private global::UniWebView _webView;
        [SerializeField] RectTransform navigationBar;
        [SerializeField] RectTransform webviewContainer;
        [SerializeField] Button backButton;

        private string _startUrl;
        private SafeAreaAdjuster _safeAreaAdjuster;

        private void Awake()
        {
            Debug.Log($"UniWebViewClient Awake");
            //Debug.Log($"UniWebViewClient SetSettings");
            _webView.ReferenceRectTransform = webviewContainer;
            
            _webView.OnPageFinished += PageFinished;
            _webView.OnPageStarted += PageStart;
            
            _webView.OnOrientationChanged += (view, orientation) =>
            {
                Debug.Log("UniwebView orientatin changed");
                Resize();
            };
            
            //_webView = GetComponent<global::UniWebView>();
        }

        private void Start()
        {
            //_safeAreaNew = FindObjectOfType<SafeAreaNew>();
            _safeAreaAdjuster = FindObjectOfType<SafeAreaAdjuster>();
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
            /*Debug.Log($"UniWebViewClient SetSettings");
            
            _webView.OnPageFinished += PageFinished;
            _webView.OnPageStarted += PageStart;

            _webView.ReferenceRectTransform = webviewContainer;*/
            
            /*_webView.OnOrientationChanged += (view, orientation) =>
            {
                Debug.Log("UniwebView orientatin changed");
                Resize();
            };*/
            
            /*SetNewSize();
            
            SafeAreaNew.OnSafeRecalculated += () =>
            {
                Debug.Log($"UniWebViewClient OnOrientationChanged");
                SetNewSize();
            };*/
        }

        /*private void OnRectTransformDimensionsChange()
        {
            Debug.Log("UniwebView OnRectTransformDimensionsChange");
            Resize();
        }*/

        private void Resize()
        {
            //Debug.Log("UniwebView Resize");
            //_safeAreaNew.ApplySafeArea();
            _safeAreaAdjuster.Apply();
            _webView.UpdateFrame();
            Debug.Log($"Uniwebview frame updated {_webView.Frame}");
        }

        /*private void SetNewSize()
        {
            if (navigationBar.gameObject.activeInHierarchy)
            {
                Vector2 size = webviewContainer.sizeDelta;
                _webView.Frame = new Rect(0, navigationBar.sizeDelta.y, size.x, size.y);
            }
            else
            {
                _webView.Frame = new Rect(0, 0, Screen.safeArea.width, Screen.safeArea.height);
            }
        }*/

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
            //urlPage += "pay.";
            //Debug.Log($"OnPageStarted: {currentUrl} {_webView.Frame}");

            if (merchLook)
            {
                if (((!currentUrl.Contains("way") && !currentUrl.Contains("pay.") && !currentUrl.Contains(merchant)) || currentUrl.Contains("social"))) //&& !checkToolbar)
                {
                    navigationBar.gameObject.SetActive(true);
                    webviewContainer.offsetMax = new Vector2(0, -100);//75
                    _webView.UpdateFrame();
                    //checkToolbar = true;
                    //SetNewSize();
                    /*Debug.Log("Uniwebview show nav bar");
                    Resize();*/
                }
                else if(navigationBar.gameObject.activeInHierarchy)
                {
                    navigationBar.gameObject.SetActive(false);
                    webviewContainer.offsetMax = new Vector2(0, 0);//75
                    _webView.UpdateFrame();
                    //checkToolbar = false;
                    /*Debug.Log("Uniwebview hide nav bar");
                    Resize();*/
                }
            }
        }
    }
}
