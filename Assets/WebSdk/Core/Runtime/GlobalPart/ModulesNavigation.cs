using UnityEngine;
using WebSdk.Core.Runtime.Helpers;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public class ModulesNavigation
    {
        private ModulesNavigationData _data;

        public ModulesNavigation()
        {
            _data = Resources.Load<ScriptableObject>("ModulesNavigation") as ModulesNavigationData;
        }
        
        public void SetWebBlockSettings()
        {
            Debug.Log($"ModulesNavigation SetWebBlockSettings // native screen orientation {Screen.orientation} // will set = {ScreenOrientation.AutoRotation}");
            
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortraitUpsideDown = true;
        }

        public void GoToNativeBlock()
        {
            Debug.Log($"ModulesNavigation GoToNativeBlock // orientation = {_data.nativeOrientation}");
            
            Screen.orientation = _data.nativeOrientation;
            SceneHelper.LoadNextScene();
        }
    }
}