using System;
using System.Diagnostics;
using Global.Helpers.Runtime;
using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.WebPart;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace WebBlock.WebBlockVariant1.Scripts
{
    public class LinkManagerMediator : IWebMediator
    {
        private IUrlLoader urlLoader;
        private IParamsManager paramsManager;
        private IWebViewClient webViewClient;
        private Stopwatch stopwatch;

        public void Init(IUrlLoader u, IParamsManager p, IWebViewClient w)
        {
            stopwatch = Stopwatch.StartNew();
            Debug.Log("WebMediatorVariant1 Init");
            
            urlLoader = u;
            paramsManager = p;
            webViewClient = w;
            
            urlLoader.SetMediator(this);
            paramsManager.SetMediator(this);
            webViewClient.SetMediator(this);
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
            urlLoader.Start();
        }
        
        public void Notify(object sender, string ev)
        {
            if (ev.Equals("OnUrlLoaded"))
            {
                paramsManager.Init();
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
            string url = WebHelper.AttachParameters(urlLoader.GetUrl(), paramsManager.GetParams());
            if (WebHelper.IsValidUrl(url))
            {
                Helper.TryLoadScene("WebviewScene");
                
                Debug.Log("open final url " + url);
                webViewClient.SetSettings();
                webViewClient.Open(url);
            }
            else
            {
                Debug.Log("not valid url " + url);
            }
            
            Debug.Log($"WebMediatorVariant1 Complete / StopWatch = {stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
            stopwatch.Stop();
        }
    }
}