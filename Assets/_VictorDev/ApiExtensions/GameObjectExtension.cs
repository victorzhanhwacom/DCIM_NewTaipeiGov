using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _VictorDev.ApiExtensions
{
    /// 原API類別功能擴充
    public static class GameObjectExtension
    {
        private static readonly Regex NameHeaderRegex = new Regex(@"^\[[^\]]+\]\s*", RegexOptions.Compiled);
        /// [Extended] - name加上標頭Header
        public static string SetNameHeader(this GameObject self, string headerName)
        {
            if (self == null || string.IsNullOrWhiteSpace(self.name)) return self?.name;
            // 移除開頭的 [XXX]（不管 XXX 是什麼）
            string baseName = NameHeaderRegex.Replace(self.name, "");
            return $"[{headerName}] {baseName}";
        }
        
        
        /// [Extended] - 刪除GameObjec (Runtime/Editor), 包含檢查是否為null
        /// <para>+ isAllowDestroyingAssets: 是否一併刪除Unity資產 (Editor環境下)</para>
        public static void SetLayerMask(this GameObject self, LayerMask layerMask)
        {
            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            self.layer = layer;
        }
        
        /// [Extended] - 刪除GameObjec (Runtime/Editor), 包含檢查是否為null
        /// <para>+ isAllowDestroyingAssets: 是否一併刪除Unity資產 (Editor環境下)</para>
        public static void ToDestroy(this GameObject self, bool isLogResult = false)
        {
            #if UNITY_EDITOR
                string selfName = self.name;
                Object.DestroyImmediate(self, false);
                if(isLogResult) global::_VictorDev.DebugUtils.Debug.Log($"GameObject: {selfName} is destroyed.", nameof(GameObjectExtension), EmojiEnum.Success);
            #else
                Object.Destroy(self);
            #endif
        }
    }
}