using UnityEngine;

namespace IvoryFox_Engine.Release_Tools.Refactor_Tools.Editor.Scripts
{
    public static class Extensions
    {
        public static Texture2D AdjustBrightness(this Texture2D imgTexture, int brightness)
        {
            ///float mappedBrightness = (51 * brightness) / 10 - 255;
                
            float mappedBrightness = brightness;
        
            Texture2D bitmapImage = new Texture2D(imgTexture.width, imgTexture.height, TextureFormat.ARGB32, false);
    
            if (mappedBrightness < -255) mappedBrightness = -255;
            if (mappedBrightness > 255) mappedBrightness = 255;
            Color color;
            for (int i = 0; i < bitmapImage.width; i++)
            {
                for (int j = 0; j < bitmapImage.height; j++)
                {
                    color = imgTexture.GetPixel(i, j);
                    float cR;
                    float cG;
                    float cB;
                    float cA;
                        
                    if(color.a == 1f)
                    {
                        cR = color.r + (mappedBrightness/255);
                        cG = color.g + (mappedBrightness/255);
                        cB = color.b + (mappedBrightness/255);
                        cA = color.a;
    
    
                        if (cR < 0) cR = 0;
                        if (cR > 255) cR = 255;
    
                        if (cG < 0) cG = 0;
                        if (cG > 255) cG = 255;
    
                        if (cB < 0) cB = 0;
                        if (cB > 255) cB = 255;
                    }else
                    {
                        cR = color.r;
                        cG = color.g;
                        cB = color.b;
                        cA = color.a;
                    }
    
                    bitmapImage.SetPixel(i, j, new Color(cR, cG, cB,cA));
                }
            }
                
            bitmapImage.Apply();
    
            return bitmapImage;
        }
    }
}