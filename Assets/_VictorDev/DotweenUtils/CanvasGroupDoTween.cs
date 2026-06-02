using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DoTweenUtils
{
    /// CanvasGroup進行DOFade
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupDoTween : MonoBehaviour
    {
        #region Variables

        [Foldout("[Event] Tween時Invoke")] public UnityEvent<bool> isEnabledEvent;
        [Foldout("[Event] Tween時Invoke")] public UnityEvent onTweenStartEvent, onTweenEndEvent;
        [Foldout("設定"), SerializeField] private float alphaOnEnabled = 1, alphaOnDisabled = 0f;
        [Foldout("設定"), SerializeField] private float duration = 0.5f, delay = 0f;
        [Foldout("設定"), SerializeField] private Ease ease = Ease.OutQuad;
        [Foldout("設定"), SerializeField] private CanvasGroup canvasGroup;
        [Foldout("設定"), SerializeField] private bool inInteractiveInTween = false;
        public bool IsOn { get; private set; }

        #endregion

        private void Awake() => ForceEnabled(false);

        public void ForceEnabled(bool isEnabled)
        {
            IsOn = isEnabled;
            canvasGroup.alpha = IsOn ? alphaOnEnabled : alphaOnDisabled;
            OnUpdateHandler();
            OnCompleteHandler();
        }
        
        public void SetEnabled(bool isEnabled)
        {
            onTweenStartEvent?.Invoke();
            IsOn = isEnabled;
            float targetAlpha = IsOn ? alphaOnEnabled : alphaOnDisabled;
            canvasGroup.DOFade(targetAlpha, duration).SetEase(ease).SetDelay(delay).OnUpdate(OnUpdateHandler).OnComplete(OnCompleteHandler);
        }

        private void OnCompleteHandler()
        {
           isEnabledEvent?.Invoke(IsOn);
           onTweenEndEvent?.Invoke();
        }

        private void OnUpdateHandler()
        {
            bool isInteractable = Mathf.Approximately(canvasGroup.alpha, 1f);
            canvasGroup.interactable = isInteractable || inInteractiveInTween;
            canvasGroup.blocksRaycasts = isInteractable || inInteractiveInTween;
        }

        private void OnValidate() => canvasGroup ??= GetComponent<CanvasGroup>();
    }
}