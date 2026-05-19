using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _VictorDev.MediatorUtils
{
    /// [Mediator] - 字串
    public class ValueMediator : MonoBehaviour
    {
        #region Variabls

        [Foldout("[Events]")] public UnityEvent<float> onValueChanged;

        [Foldout("[Settings]"), SerializeField] private int dotNumber;

        private float _currentValue;

        #endregion

        public void SetValue(int value) => SetValue((float)value);

        public void SetValue(float value)
        {
            if (Mathf.Approximately(_currentValue, value)) return;
            _currentValue = value;
            BroadCast(Calculate(_currentValue, dotNumber));
        }

        public static float Calculate(float value, float n)
            => Mathf.Round(value * Mathf.Pow(10, n)) / Mathf.Pow(10, n);

        private void BroadCast(float value)
        {
            Debug.Log($"{nameof(ValueMediator)}: {value}");
            onValueChanged?.Invoke(value);
        }
    }
}