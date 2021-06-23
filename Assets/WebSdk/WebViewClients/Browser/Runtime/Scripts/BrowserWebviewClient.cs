using System.Diagnostics;
using UnityEngine;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.WebCore;

namespace WebSdk.WebViewClients.Browser.Runtime.Scripts
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