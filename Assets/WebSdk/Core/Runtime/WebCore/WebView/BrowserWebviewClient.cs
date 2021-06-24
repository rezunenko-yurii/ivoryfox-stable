using System.Diagnostics;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.WebView
{
    public class BrowserWebviewClient : MonoBehaviour, IWebViewClient
    {
        public BrowserWebviewClient()
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
        }
        
        public IMediator Mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        public void Open(string url)
        {
            Process.Start(url);
        }

        public void SetSettings()
        {
            //throw new System.NotImplementedException();
        }

        public IModulesHost Parent { get; set; }
    }
}