using UnityEngine;
using UnityEngine.UI;

namespace VzDev.ApiExtensions
{
    public static class ScrollRectExtensions
    {
        /// ScrollBar移動到子項目所在位置
        public static void ScrollToChild(this ScrollRect scrollRect, RectTransform target)
        {
            if (scrollRect == null || scrollRect.content == null || target == null)
                return;

            Canvas.ForceUpdateCanvases(); // 避免 Layout 尚未更新導致位置錯誤

            RectTransform content = scrollRect.content;

            // 1️⃣ 目標位置與 Content 的距離（世界座標 → 本地座標）
            Vector2 localPos = content.InverseTransformPoint(target.position);
            Vector2 contentPivotPos = content.InverseTransformPoint(content.position);

            float diffY = contentPivotPos.y - localPos.y;

            // 2️⃣ 把這個 offset 轉換成 normalizedPosition
            float scrollHeight = content.rect.height - scrollRect.viewport.rect.height;

            if (scrollHeight <= 0f)
            {
                scrollRect.verticalNormalizedPosition = 1f;
                return;
            }

            // Unity 的 normalizedPosition 垂直方向：1 = 上，0 = 下 （反直覺但就是這樣）
            float normalized = diffY / scrollHeight;

            // Clamp 避免超界
            normalized = Mathf.Clamp01(normalized - 0.08f); //位移4行

            scrollRect.verticalNormalizedPosition = 1 - normalized;
        }
    }
}