using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.DotweenUtils
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public class DoTweenFader:MonoBehaviour
    {
        /*[SerializeField] private CanvasGroup canvasGroup;
        
        [Foldout("[Fade設定]"),  SerializeField] private bool isFade = true;
        [Foldout("[Fade設定]"), SerializeField, ShowIf(nameof(isFade))]
        private float fromAlpha, toAlpha = 1, duration = 0.5f,  delay;
        
        [Label("[DoTween設定]"),  SerializeField] private List<DoTweenSet> doTweenSets;
        private Sequence  sequence;

        private void Awake() => SetupDoTween();

        private void SetupDoTween()
        {
            sequence ??= DOTween.Sequence();
            doTweenSets.ForEach(tweenSet =>
            {
                switch (tweenSet.sequenceType)
                {
                    case SequenceType.Join: sequence.Join(tweenSet.GetTweenAction(transform)); break;
                    case SequenceType.Append: sequence.Append(tweenSet.GetTweenAction(transform)); break;
                }
            });
            sequence.OnPlay(() => SetInteractive(false));
            sequence.OnComplete(() => SetInteractive(true));
            sequence.OnRewind(()=>gameObject.SetActive(false));
        }

        private void SetInteractive(bool isOn)
        {
            canvasGroup.interactable = isOn;
            canvasGroup.blocksRaycasts = isOn;
        }
        
        private void OnEnable() => sequence?.PlayForward();
        public void ToDisable() => sequence?.PlayBackwards();

        private void OnValidate() => canvasGroup ??= GetComponent<CanvasGroup>();*/
    }
    
    [Serializable]
    public class DoTweenSet
    {
        public DoTweenType doTweenType;
        public SequenceType sequenceType;
        public Vector3 fromValue;
        public float duration = 0.5f, delay;
        public Ease ease = Ease.OutQuad;

        /// 取得Tween動作
        public Tween GetTweenAction(Transform target)
        {
            Tween result =  doTweenType switch
            {
                DoTweenType.Move => target.DOMove(fromValue, duration),
                DoTweenType.Rotate => target.DORotate(fromValue, duration),
                DoTweenType.Scale => target.DOScale(fromValue, duration),
                _ => null
            };
            result?.SetEase(ease).SetDelay(delay);
            return result;
        }
    }

    public enum DoTweenType
    {
        Move, Rotate, Scale
    }
    public enum SequenceType
    {
        Join, Append
    }
}