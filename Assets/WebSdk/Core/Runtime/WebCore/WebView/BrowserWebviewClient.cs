using System;
using System.Collections.Generic;
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

        public void Open(string url)
        {
            Process.Start(url);
        }

        public void SetSettings()
        {
            //throw new System.NotImplementedException();
        }

        public event Action Completed;
        public void PrepareForWork()
        {
            throw new NotImplementedException();
        }

        public void ResolveDependencies(ModulesOwner owner)
        {
            throw new NotImplementedException();
        }

        public void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}