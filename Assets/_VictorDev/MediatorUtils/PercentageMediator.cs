using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.MediatorUtils
{
    /// [Mediator] - 百分比
    public class PercentageMediator : MonoBehaviour
    {
        #region Variabls

        [Foldout("[Events]"), HideIf(nameof(isPercent01))]
        public UnityEvent<float> onPercentageChanged;

        [Foldout("[Events]"), ShowIf(nameof(isPercent01))]
        public UnityEvent<float> onNormalizedChanged;

        [Foldout("[Setting]"), SerializeField] private bool isPercent01;
        [Foldout("[Setting]"), SerializeField] private float minValue, maxValue = 100f;
        private float _currentValue;

        #endregion

        private void OnValidate()
        {
            if (minValue > maxValue) minValue = maxValue;
        }

        /// 設定值(int)
        public void SetValue(int value) => SetValue((float)value);

        /// 設定值(float)
        public void SetValue(float value)
        {
            float clampedValue = Mathf.Clamp(value, minValue, maxValue);
            if (Mathf.Approximately(_currentValue, clampedValue)) return;
            _currentValue = clampedValue;
            Broadcast(CalculatePercentage(_currentValue));
        }

        private float CalculatePercentage(float value)
        {
            float range = maxValue - minValue;
            if (Mathf.Approximately(range, 0)) return 0f;
            return (value - minValue) / range;
        }

        /// 統一發送事件
        private void Broadcast(float normalizedValue)
        {
            if (isPercent01)
                onNormalizedChanged?.Invoke(normalizedValue);
            else
                onPercentageChanged?.Invoke(normalizedValue * 100f);
        }
    }
}