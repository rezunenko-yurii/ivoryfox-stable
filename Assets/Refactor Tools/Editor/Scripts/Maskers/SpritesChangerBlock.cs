using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IvoryFox_Engine.Release_Tools.Refactor_Tools.Editor.Scripts.Maskers
{
    public partial class AssetsMaskerWindow : EditorWindow
    {
        private void SpritesChangerBlock()
        {
            EditorGUILayout.Space(20f);
            EditorGUILayout.LabelField("RESIZE ASSETS", headerStyle);

            if (GUILayout.Button("Resize all sprites"))
            {
                ResizeAll();
            }

            if (GUILayout.Button("Change sprites brightness"))
            {
                ChangeBrightness();
            }
        }

        private void ResizeAll()
        {
            IEnumerable<string> files = FindAll(new []{".png", ".jpeg"});
            
            foreach (string sFilePath in files)
            {
                Texture2D loadedImage = TextureOps.LoadImage(sFilePath);
                if( loadedImage != null )
                {
                    float w = loadedImage.width / 100f * 99f;
                    float h = loadedImage.height / 100f * 99f;
                    
                    Texture2D scaledImage = TextureOps.Scale( loadedImage, (int)w, (int)h, TextureFormat.ARGB32);

                    CompareHash(sFilePath, loadedImage, scaledImage);
                    
                    DestroyImmediate( loadedImage );
                    DestroyImmediate( scaledImage );
                }
            }
        }
        
        private void ChangeBrightness()
        {
            IEnumerable<string> files = FindAll(new []{".png", ".jpeg"});
            
            foreach (string sFilePath in files)
            {
                Texture2D loadedImage = TextureOps.LoadImage(sFilePath);
                if( loadedImage != null )
                {
                    Texture2D outTexture = loadedImage.AdjustBrightness(1);
                    TextureOps.SaveImage(outTexture, sFilePath);
                    
                    CompareHash(sFilePath, loadedImage, outTexture);
                    
                    DestroyImmediate( loadedImage );
                    DestroyImmediate( outTexture );
                }
            }
        }
        private void CompareHash(string fileName, Texture2D first, Texture2D second)
        {
            Debug.Log($"___{fileName} Hash {first.GetHashCode()}/{second.GetHashCode()} is different = {first.GetHashCode() != second.GetHashCode()}");
        }
    }
}