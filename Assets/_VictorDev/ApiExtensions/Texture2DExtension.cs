using UnityEngine;
using Debug = _VictorDev.DebugUtils.Debug;

namespace _VictorDev.ApiExtensions
{
    /// 原API類別功能擴充
    public static class Texture2DExtension
    {
        /// [Extension] - 將 Texture2D 轉成 Sprite
        public static Sprite ToSprite(this Texture2D self, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            if(self == null)
            {
                global::_VictorDev.DebugUtils.Debug.LogError("❌ Texture2D is null!");
                return null;
            }
            return Sprite.Create(self, new Rect(0,0,self.width,self.height), new Vector2(pivotX, pivotY));
        }
    }
}