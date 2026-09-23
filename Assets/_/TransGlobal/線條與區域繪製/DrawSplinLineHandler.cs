using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace VzDev.DrawUtils
{
    /// <summary>
    /// 繪製模式：
    /// <para>EvaluatePosition：沿 Spline 曲線取樣多點，適合含弧線的區段，會平滑逼近曲線形狀。</para>
    /// <para>DirectKnots：直接取用 Knot 本身座標，適合純直線折角（矩形/多邊形），
    /// 沒有多餘的取樣密度、也不需要擔心 Tangent 是否清零，效能與資料量最省。</para>
    /// </summary>
    public enum SplineDrawMode
    {
        EvaluatePosition,
        DirectKnots,
    }

    public class DrawSplinLineHandler : MonoBehaviour
    {
        #region Fields
        [Foldout("[Components]"), SerializeField] private LineRenderer lineRenderer;
        [Foldout("[Components]"), SerializeField] private SplineContainer splineContainer;

        [Foldout("[Settings]"), SerializeField, Tooltip("EvaluatePosition：沿曲線取樣，適合含弧線的形狀。DirectKnots：直接用Knot座標連線，適合純直線多邊形。")]
        private SplineDrawMode drawMode = SplineDrawMode.EvaluatePosition;

        // 每兩個 Knot 之間固定分配這麼多段，跟總 Knot 數無關，只在 EvaluatePosition 模式下才有意義
        [Foldout("[Settings]"), SerializeField, ShowIf("isEvaluateMode")] private int segmentsPerKnotSpan = 20;
        [Foldout("[Settings]"), SerializeField] private float lineWidth = 0.02f;
        private bool isDrawing => lineRenderer.positionCount > 0;
        public bool isSplineEmpty => splineContainer.Spline.Count == 0;
        private bool isEvaluateMode => drawMode == SplineDrawMode.EvaluatePosition;
        #endregion

        [Button, HideIf("isSplineEmpty")]
        public void DrawLine()
        {
            if (!NullCheck()) return;

            var spline = splineContainer.Spline;
            bool isClosed = spline.Closed;

            var pointList = drawMode == SplineDrawMode.EvaluatePosition
                ? SampleByEvaluatePosition(spline, isClosed)
                : SampleByDirectKnots(spline);

            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());
            lineRenderer.loop = isClosed;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }

        /// <summary>
        /// 沿 Spline 曲線密集取樣，逼近實際曲線形狀（含弧線段時使用此模式）。
        /// 【Local Space】EvaluatePosition 回傳的是世界座標，這裡透過 InverseTransformPoint
        /// 換算回相對於 splineContainer 自身 Transform 的 Local 座標，才能配合
        /// lineRenderer.useWorldSpace = false，讓畫出來的線跟著 GameObject 的移動/旋轉/縮放走。
        /// </summary>
        private System.Collections.Generic.List<Vector3> SampleByEvaluatePosition(Spline spline, bool isClosed)
        {
            int knotCount = spline.Count;
            int spanCount = isClosed ? knotCount : knotCount - 1;

            var pointList = new System.Collections.Generic.List<Vector3>();
            Transform t = splineContainer.transform;

            for (int span = 0; span < spanCount; span++)
            {
                for (int i = 0; i < segmentsPerKnotSpan; i++)
                {
                    // 用 SplineUtility 換算全域 t，確保每一段都拿到相同的取樣密度
                    float localT = i / (float)segmentsPerKnotSpan;
                    float globalT = (span + localT) / spanCount;
                    float3 worldPos = splineContainer.EvaluatePosition(globalT);
                    pointList.Add(t.InverseTransformPoint(worldPos));
                }
            }
            // 補上最後一個 Knot 本身的精確位置（收尾），Closed 時交由 lineRenderer.loop 處理，不需要額外補點
            if (!isClosed)
            {
                float3 lastWorldPos = splineContainer.EvaluatePosition(1f);
                pointList.Add(t.InverseTransformPoint(lastWorldPos));
            }

            return pointList;
        }

        /// <summary>
        /// 直接取用每個 Knot 座標連線，不經過曲線插值。
        /// 適用於純直線多邊形（矩形等），前提是所有 Knot 的 Tangent 皆為 0（Linear），
        /// 否則會忽略 Spline 實際想表達的弧度，畫出來的會是抄近路的折線捷徑。
        /// 【Local Space】Knot.Position 本身就已經是相對於 splineContainer 的 Local 座標，
        /// 不需要再做任何轉換，直接使用即可。
        /// </summary>
        private System.Collections.Generic.List<Vector3> SampleByDirectKnots(Spline spline)
        {
            var pointList = new System.Collections.Generic.List<Vector3>(spline.Count);
            for (int i = 0; i < spline.Count; i++)
            {
                pointList.Add((Vector3)spline[i].Position);
            }
            return pointList;
        }

        public void UpdateLineWidth(float newWidth)
        {
            lineWidth = newWidth;
            if (isDrawing)
            {
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
            }
        }

        private void OnValidate()
        {
            if (NullCheck())
            {
                // Local Space：讓畫出來的線跟著 GameObject 的 Transform 走，
                // 與 SplineContainer 本身「即時依 Transform 換算」的行為保持一致。
                lineRenderer.useWorldSpace = false;
            }
            if (isDrawing)
            {
                UpdateLineWidth(lineWidth);
            }
        }

        [Button, ShowIf("isDrawing")]
        public void ClearLine() => lineRenderer.positionCount = 0;

        [Button, HideIf("isSplineEmpty")]
        public void ClearSpline()
        {
            splineContainer.Spline.Clear();
            ClearLine();
        }

        private bool NullCheck()
        {
            if (splineContainer == null || lineRenderer == null)
            {
                Debug.LogError("SplineContainer or LineRenderer is not assigned.");
                return false;
            }

            if (splineContainer.Spline.Count == 0)
            {
                Debug.LogError("SplineContainer has no points to draw.", this);
                return false;
            }

            return true;
        }

        #region Lifecycle — 自動監聽 Spline 修改，Editor 下編輯節點即時反映到畫面
        private void OnEnable() => Spline.Changed += HandleSplineChanged;

        private void OnDisable() => Spline.Changed -= HandleSplineChanged;

        /// <summary>
        /// Spline.Changed 是靜態事件，場景中每一條 Spline 被修改都會觸發，
        /// 所以要先判斷是不是自己這個 SplineContainer 底下的 Spline，
        /// 避免場景中其他物件的 Spline 異動也誤觸這裡重畫。
        /// </summary>
        private void HandleSplineChanged(Spline spline, int knotIndex, SplineModification modification)
        {
            if (splineContainer == null) return;
            if (spline != splineContainer.Spline) return;
            if (isSplineEmpty) return;
            DrawLine();
        }
        #endregion

    }
}