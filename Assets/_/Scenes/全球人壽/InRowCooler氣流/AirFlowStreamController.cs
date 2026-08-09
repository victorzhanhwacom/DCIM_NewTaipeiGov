using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace VzDev.RenderingUtils.AirFlow
{
    /// <summary>
    /// 掛載於「獨立氣流路徑物件」——從 InRowCooler 機身延伸出去的一段 Mesh，
    /// 不是機身本體的一部分，不會被 MaterialReplacer 等以
    /// GetComponentsInChildren&lt;Renderer&gt; 遍歷機身子物件的系統誤判為機身的一部分
    /// （同樣的隔離思路沿用自 BoundingBoxHighlightController）。
    ///
    /// 捲動完全交給 Shader 用 _Time.y 計算，這裡只在資料變動時透過
    /// MaterialPropertyBlock 設定一次 Uniform，不逐幀更新，
    /// 大量 InRowCooler 同時存在時不會產生額外 Update 開銷。
    ///
    /// 顏色支援起訖漸變(flowColorStart→flowColorEnd)，沿物件長度方向插值，
    /// 例如可做出「靠機身端亮青色、遠端漸淡成偏白/透明」的層次感。
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class AirFlowStreamController : MonoBehaviour
    {
        #region Fields
        [Foldout("[Components]"), SerializeField] private MeshRenderer targetRenderer;

        [Foldout("[Settings]"), SerializeField, ColorUsage(true, true), Tooltip("起點(靠機身端)顏色")]
        private Color flowColorStart = new Color(0.4f, 0.9f, 1f, 1f);
        [Foldout("[Settings]"), SerializeField, ColorUsage(true, true), Tooltip("終點(遠離機身端)顏色")]
        private Color flowColorEnd = new Color(0.4f, 0.9f, 1f, 1f);
        [Foldout("[Settings]"), SerializeField, Range(0f, 1f)] private float maxOpacity = 0.8f;
        [Foldout("[Settings]"), SerializeField, Tooltip("沿氣流方向的貼圖重複次數，依物件實際長度手動調整")]
        private float tilingY = 4f;
        [Foldout("[Settings]"), SerializeField] private float scrollSpeed = 1f;
        [Foldout("[Settings]"), SerializeField, Tooltip("true=沿UV正方向(通常代表排風/出風)，false=反向(代表回風/入風)")]
        private bool isExhaustDirection = true;

        [Foldout("[Fade]"), SerializeField, Range(0.001f, 0.5f)] private float fadeInRange = 0.15f;
        [Foldout("[Fade]"), SerializeField, Range(0.001f, 0.5f)] private float fadeOutRange = 0.35f;
        [Foldout("[Fade]"), SerializeField, Range(0.05f, 2f), Tooltip("SetFlowing切換開/關時的淡入淡出時間")]
        private float toggleFadeDuration = 0.4f;

        [SerializeField, ReadOnly, Tooltip("目前氣流是否處於開啟狀態")] private bool isFlowing = true;

        private MaterialPropertyBlock mpb;
        private float currentOpacityMultiplier = 1f;

        private static readonly int ColorID = Shader.PropertyToID("_Color");
        private static readonly int ColorEndID = Shader.PropertyToID("_ColorEnd");
        private static readonly int OpacityID = Shader.PropertyToID("_Opacity");
        private static readonly int TilingYID = Shader.PropertyToID("_TilingY");
        private static readonly int ScrollSpeedID = Shader.PropertyToID("_ScrollSpeed");
        private static readonly int FlowDirectionID = Shader.PropertyToID("_FlowDirection");
        private static readonly int FadeInRangeID = Shader.PropertyToID("_FadeInRange");
        private static readonly int FadeOutRangeID = Shader.PropertyToID("_FadeOutRange");
        #endregion

        #region Lifecycle
        private void OnEnable()
        {
            if (targetRenderer == null) targetRenderer = GetComponent<MeshRenderer>();
            mpb ??= new MaterialPropertyBlock();
            ApplyAllProperties();
        }

        /// <summary>
        /// Inspector上調整參數時Play模式下即時反應，不需要重新Play。
        /// </summary>
        private void OnValidate()
        {
            if (targetRenderer == null) targetRenderer = GetComponent<MeshRenderer>();
            if (!Application.isPlaying) return;
            mpb ??= new MaterialPropertyBlock();
            ApplyAllProperties();
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }
        #endregion

        #region Public API — 供外部(例如Cooler運轉狀態變化時)呼叫
        /// <summary>
        /// 切換氣流開/關，透過DOTween淡入淡出_Opacity，不是直接SetActive，
        /// 避免視覺上瞬間消失/出現造成跳動感。
        /// </summary>
        public void SetFlowing(bool flowing)
        {
            if (isFlowing == flowing) return;
            isFlowing = flowing;

            DOTween.Kill(this);
            DOTween.To(() => currentOpacityMultiplier, x =>
                {
                    currentOpacityMultiplier = x;
                    ApplyOpacity();
                }, flowing ? 1f : 0f, toggleFadeDuration)
                .SetTarget(this);
        }

        /// <summary>
        /// 切換氣流方向（例如同一顆Cooler模式切換成回風偵測用途時）。
        /// </summary>
        public void SetFlowDirection(bool exhaust)
        {
            isExhaustDirection = exhaust;
            ApplyFlowDirection();
        }

        /// <summary>
        /// 執行期調整起訖漸變顏色，供狀態變化情境使用
        /// （例如告警發生時把終點色從青色改成紅色，強化警示感）。
        /// </summary>
        public void SetGradientColors(Color start, Color end)
        {
            flowColorStart = start;
            flowColorEnd = end;
            ApplyColors();
        }
        #endregion

        #region Apply to MaterialPropertyBlock
        private void ApplyAllProperties()
        {
            if (targetRenderer == null) return;

            targetRenderer.GetPropertyBlock(mpb);
            mpb.SetColor(ColorID, flowColorStart);
            mpb.SetColor(ColorEndID, flowColorEnd);
            mpb.SetFloat(TilingYID, tilingY);
            mpb.SetFloat(ScrollSpeedID, scrollSpeed);
            mpb.SetFloat(FadeInRangeID, fadeInRange);
            mpb.SetFloat(FadeOutRangeID, fadeOutRange);
            mpb.SetFloat(FlowDirectionID, isExhaustDirection ? 1f : -1f);
            mpb.SetFloat(OpacityID, maxOpacity * currentOpacityMultiplier);
            targetRenderer.SetPropertyBlock(mpb);
        }

        private void ApplyColors()
        {
            if (targetRenderer == null) return;
            targetRenderer.GetPropertyBlock(mpb);
            mpb.SetColor(ColorID, flowColorStart);
            mpb.SetColor(ColorEndID, flowColorEnd);
            targetRenderer.SetPropertyBlock(mpb);
        }

        private void ApplyOpacity()
        {
            if (targetRenderer == null) return;
            targetRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(OpacityID, maxOpacity * currentOpacityMultiplier);
            targetRenderer.SetPropertyBlock(mpb);
        }

        private void ApplyFlowDirection()
        {
            if (targetRenderer == null) return;
            targetRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(FlowDirectionID, isExhaustDirection ? 1f : -1f);
            targetRenderer.SetPropertyBlock(mpb);
        }
        #endregion
    }
}