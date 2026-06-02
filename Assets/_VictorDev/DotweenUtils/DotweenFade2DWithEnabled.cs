using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace VzDev.DoTweenUtils
{
    public class DotweenFade2DWithEnabled : MonoBehaviour
    {
        [Header("基本設定")]
        [SerializeField] private bool isOnEnabled = true;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private bool isRandomDelay = true;
        [SerializeField] private float delay = 0.3f;
        [SerializeField] private float delay_Start = 0f;
        [SerializeField] private Ease ease = Ease.OutQuad;

        [Header("移動設定")]
        [SerializeField] private bool isDoMove = false;
        [SerializeField] private Vector3 fromPosValue = Vector3.zero;

        [Header("縮放設定")]
        [SerializeField] private bool isDoScale = false;
        [SerializeField] private float fromScaleValue = 1f;

        [Header("目標（留空自抓）")]
        [SerializeField] private Transform targetTrans;

        private Vector3 originalPos;
        private Vector3 originalScale;
        private CanvasGroup cg;
        private RectTransform rect;
        private Sequence seq;

        [Header("事件")]
        public UnityEvent onAnimateFinished = new UnityEvent();
        public UnityEvent onEnabledEvent = new UnityEvent();
        public UnityEvent onDisabledEvent = new UnityEvent();

        private void Awake()
        {
            if (targetTrans == null) targetTrans = transform;

            rect = targetTrans as RectTransform;
            originalPos = rect.localPosition;
            originalScale = rect.localScale;

            if (!targetTrans.TryGetComponent(out cg))
                cg = targetTrans.gameObject.AddComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            onEnabledEvent?.Invoke();
            if (isOnEnabled) ToShow();
        }

        public void ToShow()
        {
            KillSeq();

            float targetDelay = delay_Start + (isRandomDelay ? Random.Range(0f, Mathf.Max(0f, delay)) : Mathf.Max(0f, delay));
            // ===== 建立 Sequence =====
            seq = DOTween.Sequence().SetTarget(rect);

            // 先插入延遲（比 SetDelay 更直觀可靠）
            if (targetDelay > 0f)
                seq.AppendInterval(targetDelay);

            // Fade (作為 Append 的第一個 tween，其他用 Join 與它同步)
            cg.alpha = 0;
            seq.Append(cg.DOFade(1f, duration).SetEase(ease));

            // Move
            if (isDoMove)
            {
                Vector3 fromPos = originalPos + fromPosValue;
                rect.localPosition = fromPos;
                seq.Join(rect.DOLocalMove(originalPos, duration).SetEase(ease));
            }

            // Scale
            if (isDoScale)
            {
                rect.localScale = Vector3.one * fromScaleValue;
                seq.Join(rect.DOScale(originalScale, duration).SetEase(ease));
            }

            seq.OnComplete(() => onAnimateFinished?.Invoke());
        }


        private void OnDisable()
        {
            KillSeq();
            onDisabledEvent?.Invoke();
        }

        private void KillSeq()
        {
            seq?.Kill(true);
            seq = null;
        }
    }
}
