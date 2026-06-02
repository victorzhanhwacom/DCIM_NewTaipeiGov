using UnityEngine;

namespace VzDev.ApiExtensions
{
    public static class ImageExtension
    {

        /// [Extended] -  依Gradient內容產生Texture
        public static Texture2D CreateTexture2D(this Gradient self, int width = 256, int height = 1)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp
            };
            for (int x = 0; x < width; x++)
            {
                Color col = self.Evaluate((float)x / (width - 1));
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, col);
                }
            }
            texture.Apply();
            return texture;
        }

        
    }
}