using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _VictorDev.DebugUtils
{
    public class DotweenMover : TweenEffectBase
    {
        [Foldout("[Setting]"), SerializeField] private bool saveOriginalValue = true;
        [Foldout("[Setting]"), SerializeField] private TweenMethod tweenMethod;
        [Foldout("[Setting]"), SerializeField] private TweenValueType valueType;
        [Foldout("[Setting]"), SerializeField] private Vector2 value;

        [Foldout("[Events]")] public UnityEvent onComplete;
        
        private RectTransform _rectTransform;

        private Vector2 _originalPosition;
        
        private TweenerCore<Vector2, Vector2, VectorOptions> _tween;

        private void Awake()
        {
            _rectTransform ??= GetComponent<RectTransform>();
            if (saveOriginalValue) _originalPosition = _rectTransform.anchoredPosition;
        }


        [Button]
        public void Play()
        {
            if(_tween != null) _tween.Kill();
            
            switch (tweenMethod)
            {
                case TweenMethod.TweenTo:
                    _tween = _rectTransform.DOAnchorPos(value, tweenBaseProperty.duration).From(_originalPosition);
                    break;
                case TweenMethod.TweenFrom:
                    _tween = _rectTransform.DOAnchorPos(_originalPosition, tweenBaseProperty.duration).From(value);
                    break;
            }
            _tween = _tween.SetEase(tweenBaseProperty.ease);
            if (tweenBaseProperty.delay > 0) _tween = _tween.SetDelay(tweenBaseProperty.delay);
            if (valueType == TweenValueType.Relative) _tween = _tween.SetRelative();
        }
    }
}