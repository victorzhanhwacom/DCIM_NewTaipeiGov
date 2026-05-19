using UnityEngine;
using UnityEngine.UI;

namespace _VictorDev.ObjectUtils
{
    public static class UiHelper
    {
        /// 限制UI在螢幕(Canvas)範圍裡，支援修改過的Pivot、Layout
        public static Vector2 ClampUIToScreen(Vector2 localPos, RectTransform uiTarget,
            RectTransform canvasRectTransform)
        {
            if (uiTarget == null || canvasRectTransform == null)
                return localPos;

            // 如果是使用 layout 元件，確保尺寸已經更新（選擇性，會有性能成本）
            LayoutRebuilder.ForceRebuildLayoutImmediate(uiTarget);

            // 保存原本位置，之後還原（避免副作用）
            Vector3 originalLocalPos = uiTarget.localPosition;

            // 把 UI 暫放在候選位置（localPosition），並求出 world corners
            uiTarget.localPosition = localPos;
            Vector3[] worldCorners = new Vector3[4];
            uiTarget.GetWorldCorners(worldCorners);

            // 把 world corners 轉回 canvas 的 local 空間
            Vector3[] canvasLocalCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
                canvasLocalCorners[i] = canvasRectTransform.InverseTransformPoint(worldCorners[i]);

            // canvas 的顯示範圍（以 local 空間座標）— rect 的中心是 (0,0)
            Rect canvasRect = canvasRectTransform.rect;
            float canvasMinX = canvasRect.xMin;
            float canvasMaxX = canvasRect.xMax;
            float canvasMinY = canvasRect.yMin;
            float canvasMaxY = canvasRect.yMax;

            // 找出 UI 的 min/max（以 canvas local 為基準）
            float uiMinX = canvasLocalCorners[0].x;
            float uiMinY = canvasLocalCorners[0].y;
            float uiMaxX = canvasLocalCorners[0].x;
            float uiMaxY = canvasLocalCorners[0].y;
            for (int i = 1; i < 4; i++)
            {
                uiMinX = Mathf.Min(uiMinX, canvasLocalCorners[i].x);
                uiMinY = Mathf.Min(uiMinY, canvasLocalCorners[i].y);
                uiMaxX = Mathf.Max(uiMaxX, canvasLocalCorners[i].x);
                uiMaxY = Mathf.Max(uiMaxY, canvasLocalCorners[i].y);
            }

            // 計算需要的位移（以 canvas local 為單位）
            float shiftX = 0f;
            if (uiMinX < canvasMinX) shiftX = canvasMinX - uiMinX;
            else if (uiMaxX > canvasMaxX) shiftX = canvasMaxX - uiMaxX;

            float shiftY = 0f;
            if (uiMinY < canvasMinY) shiftY = canvasMinY - uiMinY;
            else if (uiMaxY > canvasMaxY) shiftY = canvasMaxY - uiMaxY;

            // 把位移從 canvas local 空間轉換回 uiTarget 要加的 localPosition（因為我們是直接操控 uiTarget.localPosition, canvas 與 uiTarget 共享相同父空間）
            Vector2 adjustedLocalPos = localPos + new Vector2(shiftX, shiftY);

            // 還原原本位置（避免副作用）
            uiTarget.localPosition = originalLocalPos;

            return adjustedLocalPos;
        }
    }
}