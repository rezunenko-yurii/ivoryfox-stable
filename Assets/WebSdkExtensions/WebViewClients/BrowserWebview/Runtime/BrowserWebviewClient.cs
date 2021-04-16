using System.Diagnostics;
using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.WebPart;
using UnityEngine;

namespace WebSdkExtensions.WebViewClients.BrowserWebview.Runtime
{
    public class BrowserWebviewClient : IWebViewClient
    {
        public BrowserWebviewClient()
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
        }
        
        public IMediator mediator { get; private set; }
        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void Open(string url)
        {
            Process.Start(url);
        }

        public void SetSettings()
        {
            //throw new System.NotImplementedException();
        }
    }
}