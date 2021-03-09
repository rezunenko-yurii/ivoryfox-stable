using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Refactor_Tools.Editor.Scripts.Maskers
{
    public partial class AssetsMaskerWindow
    {
        private GUIStyle headerStyle;
        
        [MenuItem("IvoryFox/Assets Masker")]
        public static void ShowWindow()
        {
            GetWindow<AssetsMaskerWindow>("AssetsMasker");
        }

        private void Awake()
        {
            headerStyle = new GUIStyle {alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold};
        }

        private void OnGUI()
        {
            RenamerSearchBlock();
            SpritesChangerBlock();
            ClassSpawnerBlock();
        }
        
        private IEnumerable<string> FindAll(string[] extensions)
        {
            IEnumerable<string> files = Directory.GetFiles("Assets/", "*.*", SearchOption.AllDirectories)
                .Where(s => extensions.Any(s.EndsWith));
            
            return files;
        }
    }
}