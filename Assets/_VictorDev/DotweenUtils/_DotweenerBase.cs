using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.DoTweenUtils
{
    /// Dotweener基底, 設定duration與ease效果
    public abstract class _DotweenerBase : MonoBehaviour
    {
        [Foldout("設定Tweener")] [SerializeField] private float duration = 0.15f;
        private bool IsHaveDuration => duration > 0;
        protected float Duration => duration;

        [Foldout("設定Tweener"), ShowIf(nameof(IsHaveDuration))] [SerializeField]
        private Ease ease = Ease.OutQuad;

        [Foldout("設定Tweener")] [SerializeField] private float delay = 0f;
        private bool IsDelay => delay > 0;

        [Foldout("設定Tweener"), ShowIf(nameof(IsDelay))] [SerializeField]
        private bool isRandomDelay = true;

        /// 設定Delay與Ease效果
        protected Tween SetDelayAndEase(Tween doTween) =>
            doTween.SetDelay(isRandomDelay ? Random.Range(0, delay) : delay).SetEase(ease);
    }
}