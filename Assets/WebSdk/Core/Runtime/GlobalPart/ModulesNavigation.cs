using UnityEngine;
using WebSdk.Core.Runtime.Helpers;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public class ModulesNavigation
    {
        private ScreenOrientation _nativeScreenOrientation;
        
        public void SetWebBlockSettings()
        {
            Debug.Log($"ModulesNavigation SetWebBlockSettings // native screen orientation {Screen.orientation}");
            
            _nativeScreenOrientation = Screen.orientation;
            
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        public void GoToNativeBlock()
        {
            Debug.Log($"ModulesNavigation GoToNativeBlock");
            
            Screen.orientation = _nativeScreenOrientation;
            SceneHelper.LoadNextScene();
        }
    }
}