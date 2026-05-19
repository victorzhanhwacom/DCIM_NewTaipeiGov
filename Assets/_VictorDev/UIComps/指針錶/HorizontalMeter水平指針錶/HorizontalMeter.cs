using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.UIComps
{
    public class HorizontalMeter : MonoBehaviour
    {
        [ReadOnly, SerializeField] private float currentValue;
        public float CurrentValue => currentValue;

        [Foldout("[設定]"), SerializeField] private float middleValue = 0f;
        [Foldout("[設定]"), SerializeField] private float offsetValue = 100f;
        [Foldout("[設定]"), SerializeField] private float offSetPosX = 65f;
        [Foldout("[設定]"), SerializeField] private float duration = 0.5f;
        [Foldout("[設定]"), SerializeField] private RectTransform needleDot;

        public void SetValue(float value)
        {
            currentValue = value;
            UpdateUI();
        }

        private void UpdateUI()
        {
            float percentage = (currentValue - middleValue) / offsetValue;
            percentage = Mathf.Clamp(percentage, -1f, 1f);
            float needlePosX = percentage * offSetPosX;
            //needleDot.DOLocalMoveX(needlePosX, duration).SetEase(Ease.InOutQuad);
            needleDot.DOAnchorPos3DX(needlePosX, duration).SetEase(Ease.InOutQuad);
        }
    }
}