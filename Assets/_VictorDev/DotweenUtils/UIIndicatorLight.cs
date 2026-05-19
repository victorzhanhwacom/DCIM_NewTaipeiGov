using System;
using _VictorDev.Configs;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _VictorDev.DoTweenUtils
{
    /// UI 指示燈特效
    public class UIIndicatorLight : MonoBehaviour
    {
        #region Variables

        [Foldout("[設定]"), SerializeField]private EnumIndicatorStatus indicatorMode = EnumIndicatorStatus.Normal;
        [Foldout("[設定]"), SerializeField] private Image imgIndicator;
        
        private Tween currentTween;
        private Color sourceColor;

        #endregion

        /// 設定Indicator模式
        public void SetMode(EnumIndicatorStatus mode)
        {
            indicatorMode = mode;

            StopTween();
            switch (indicatorMode)
            {
                case EnumIndicatorStatus.Normal:
                    PlayNormal();
                    break;

                case EnumIndicatorStatus.Warning:
                    PlayWarning();
                    break;

                case EnumIndicatorStatus.Overload:
                    PlayOverload();
                    break;
            }
        }

        #region Dotween樣式

        /// Normal（平穩呼吸閃爍）
        private void PlayNormal()
        {
            StopTween();
            currentTween = imgIndicator
                .DOFade(0.2f, 0.8f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        /// Alert（快速閃爍）
        private void PlayWarning()
        {
            StopTween();
            currentTween = imgIndicator
                .DOFade(0f, 0.25f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }

        /// Error（雙閃）
        /// <para>+ Pattern：亮 → 暗 → 亮 → 暗 → 停 0.4 秒 → 重複</para>
        private void PlayOverload()
        {
            StopTween();
            var seq = DOTween.Sequence();

            seq.Append(imgIndicator.DOFade(0f, 0.1f));
            seq.Append(imgIndicator.DOFade(1f, 0.1f));

            seq.Append(imgIndicator.DOFade(0f, 0.1f));
            seq.Append(imgIndicator.DOFade(1f, 0.1f));

            seq.AppendInterval(1f); // 讓警告更有節奏感

            seq.SetLoops(-1);
            currentTween = seq;
        }

        #endregion

        public void StopTween()
        {
            currentTween?.Kill();
            currentTween = null;
            imgIndicator.color = sourceColor;
        }

        private void Awake() => sourceColor = imgIndicator.color;

        private void Reset() => imgIndicator = GetComponent<Image>();

        private void OnEnable() => SetMode(indicatorMode);

        private void OnDisable() => StopTween();
    }
}