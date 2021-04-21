using System.Diagnostics;
using UnityEngine;
using WebSdk.Core.Runtime.Helpers.Scripts;
using WebSdk.Core.Runtime.WebCore;
using Debug = UnityEngine.Debug;

namespace WebSdk.WebManagers.Default.Runtime.Scripts
{
    public class WebManagerMediator : IWebMediator
    {
        private IUrlLoader _urlLoader;
        private IParamsManager _paramsManager;
        private IWebViewClient _webViewClient;
        private Stopwatch _stopwatch;

        public void Init(IUrlLoader u, IParamsManager p, IWebViewClient w)
        {
            _stopwatch = Stopwatch.StartNew();
            Debug.Log("WebMediatorVariant1 Init");
            
            _urlLoader = u;
            _paramsManager = p;
            _webViewClient = w;
            
            _urlLoader.SetMediator(this);
            _paramsManager.SetMediator(this);
            _webViewClient.SetMediator(this);
        }
        
        public void Start()
        {
            /*if (GlobalFacade.notification.IsUsing())
            {
                Debug.Log($"Using notifications");
                if (GlobalFacade.notification.IsReady())
                {
                    Debug.Log($"App was opened with notifications / Start url loader");
                    StartPoint();
                }
                else
                {
                    Debug.Log($"Will wait for notification is ready");
                    GlobalFacade.notification.OnReady += StartPoint;
                }
            }
            else
            {
                StartPoint();
            }*/
            
            StartPoint();
        }

        private void StartPoint()
        {
            //Helper.TryLoadScene("WebviewScene");
            _urlLoader.Start();
        }
        
        public void Notify(object sender, string ev)
        {
            if (ev.Equals("OnUrlLoaded"))
            {
                _paramsManager.Init();
            }
            else if (ev.Equals("OnParamsLoaded"))
            {
                StartWebview();
            }
            else if (ev.Equals("Error"))
            {
                Debug.Log("WebMediatorVariant1 Notify catch error");
            }
        }

        private void StartWebview()
        {
            Debug.Log("WebMediatorVariant1 StartWebview");
            string url = WebHelper.AttachParameters(_urlLoader.GetUrl(), _paramsManager.GetParams());
            if (WebHelper.IsValidUrl(url))
            {
                Helper.TryLoadScene("WebviewScene");
                
                Debug.Log("open final url " + url);
                _webViewClient.SetSettings();
                _webViewClient.Open(url);
            }
            else
            {
                Debug.Log("not valid url " + url);
            }
            
            Debug.Log($"WebMediatorVariant1 Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
            _stopwatch.Stop();
        }
    }
}