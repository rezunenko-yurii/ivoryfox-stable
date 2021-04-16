using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.WebPart;
using UnityEngine;

namespace WebSdkExtensions.WebViewClients.UniWebView.Runtime
{
    public class UniWebViewClient : IWebViewClient
    {
        private global::UniWebView webView;
        private bool isEscape;
        
        private const string ClickAgainToExit = "click again to exit";
        private const int TimeBetweenDoubleClick = 2;
    
        public UniWebViewClient()
        {
            webView = GlobalFacade.monoBehaviour.gameObject.AddComponent<global::UniWebView>();
        }

        private void ChangeOrientation(global::UniWebView webview, ScreenOrientation orientation) => webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        private void DisableDoubleClick() => isEscape = false;
    
        private bool OnShouldClose(global::UniWebView webview)
        {
            if (isEscape)
            {                        
                Close();
                Application.Quit();
                return true;
            }
            else
            {
                isEscape = true;
            
                if (!webView.IsInvoking(nameof(DisableDoubleClick)))
                {
                    //Helper.ShowToast(ClickAgainToExit);
                    webView.Invoke(nameof(DisableDoubleClick), TimeBetweenDoubleClick);
                }
            }

            return false;
        }
    
        private void Close() 
        {
            if(webView.IsInvoking(nameof(DisableDoubleClick)))
            {
                webView.CancelInvoke(nameof(DisableDoubleClick));
            }
        
            webView.CleanCache();
            webView = null;
        }

        public void Open(string url)
        {
            webView.Load(url);
            webView.Show();
        }

        public void SetSettings()
        {
            webView.SetContentInsetAdjustmentBehavior(UniWebViewContentInsetAdjustmentBehavior.Always);
            //uniWV.SetImmersiveModeEnabled(true);
            webView.SetBackButtonEnabled(true);
            webView.Frame = new Rect(0, 0, Screen.safeArea.width, Screen.safeArea.height);
            webView.OnOrientationChanged += ChangeOrientation;
            webView.OnShouldClose += OnShouldClose;

            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
        }

        public IMediator mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
