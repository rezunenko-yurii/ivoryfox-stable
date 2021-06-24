using UnityEngine;

namespace WebSdk.Core.Runtime.Global
{
    public class ModulesNavigationData : ScriptableObject
    {
        [HideInInspector] public ScreenOrientation webviewOrientation = ScreenOrientation.AutoRotation;
        public ScreenOrientation nativeOrientation;
    }
}