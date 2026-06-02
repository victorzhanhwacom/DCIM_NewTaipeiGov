using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DebugUtils
{
    public class DotweenFader : TweenEffectBase
    {
        [Foldout("[Setting]"), SerializeField] private TweenMethod tweenMethod;
        [Foldout("[Setting]"), SerializeField] private TweenValueType valueType;
        [Foldout("[Setting]"), SerializeField] private float value;

        [Foldout("[Events]")] public UnityEvent onComplete;
        
        [Foldout("[Components]"), SerializeField]
        private CanvasGroup canvasGroup;

        [Button]
        public void Play()
        {
            TweenerCore<float, float, FloatOptions> tween = 
                canvasGroup.DOFade(value, tweenBaseProperty.duration).SetEase(tweenBaseProperty.ease);
            if (tweenMethod == TweenMethod.TweenFrom) tween = tween.From();
            if (tweenBaseProperty.delay > 0) tween = tween.SetDelay(tweenBaseProperty.delay);
            if (valueType == TweenValueType.Relative) tween.SetRelative();
        }

        private void OnValidate() => canvasGroup ??= GetComponent<CanvasGroup>();
    }
}