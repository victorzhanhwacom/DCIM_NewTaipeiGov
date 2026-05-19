using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _VictorDev.DoTweenUtils
{
    public class AlphaDotweener : MonoBehaviour
    {
        [Foldout("[Event] - 結束時Invoke")]
        public UnityEvent onComplete;
        
        public void ToAlpha(float alpha)
        {
            _tweener?.Kill();
            _tweener = canvasGroup.DOFade(alpha, duration).SetDelay(delay).SetEase(easeType).OnComplete(onComplete.Invoke);
        }

        private Tweener _tweener;

        [Foldout("[設定]")] [SerializeField] private float duration = 0.15f, delay = 0;
        [Foldout("[設定]")] [SerializeField] private Ease easeType = Ease.OutQuad;

        private CanvasGroup canvasGroup => _canvasGroup ??= GetComponentInParent<CanvasGroup>(true);
        [NonSerialized] private CanvasGroup _canvasGroup;
    }
}