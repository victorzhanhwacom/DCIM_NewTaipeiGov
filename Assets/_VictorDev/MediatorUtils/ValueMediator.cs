using System;
using System.Globalization;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using VzDev.UnityAPI.Extensions;

namespace VzDev.MediatorUtils
{
    /// <summary>
    /// 數值轉換器
    /// <summary>
    public class ValueMediator : MonoBehaviour
    {
        #region UnityEvents

        [SerializeField] private EnumOutputValueType outputValueType = EnumOutputValueType.Percent;
        private bool IsPercent => outputValueType == EnumOutputValueType.Percent || outputValueType == EnumOutputValueType.Percent01;

        [Foldout("[Values]"), SerializeField, ReadOnly] private float lastValue, lastValueFixed;
        [Foldout("[Events] - string")] public UnityEvent<string> valueToStringEvent;
        [Foldout("[Events] - float")] public UnityEvent<float> valueToFloatEvent, valueToFloat01Event;
        [Foldout("[Events] - int")] public UnityEvent<int> valueToIntEvent;
        #endregion

        #region Field
        [Foldout("[Settings]"), SerializeField, ShowIf("IsPercent")] private Vector2 minMax = new Vector2(0f, 1f);
        [Foldout("[Settings]"), SerializeField, Min(0), OnValueChanged("OnHaveSeparatorChanged")] private int decimalPlaces = 1;
        [Foldout("[Settings]"), SerializeField, HideIf("IsPercent"), OnValueChanged("OnHaveSeparatorChanged")] private bool haveSeparator = true;
        private void OnHaveSeparatorChanged() => InvokeValue(lastValueFixed);
        #endregion

        #region SetValue 各類型值, 先將所有數值轉換為 float, 再進行換算處理
        public void SetValue(string value)
        {
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatValue))
                SetValue(floatValue);
            else
                Debug.LogWarning($"ValueMediator: Unable to parse '{value}' to float.");
        }
        public void SetValue(int value) => SetValue((float)value);
        public void SetValue(float value)
        {
            if (Mathf.Approximately(lastValue, value)) return;
            lastValue = value;
            ValueHandler();
        }

        private void ValueHandler()
        {
            switch (outputValueType)
            {
                case EnumOutputValueType.Percent:
                    lastValueFixed = Mathf.InverseLerp(minMax.x, minMax.y, lastValue) * 100f;
                    break;
                case EnumOutputValueType.Percent01:
                    lastValueFixed = Mathf.InverseLerp(minMax.x, minMax.y, lastValue);
                    break;
                case EnumOutputValueType.Value:
                    lastValueFixed = lastValue;
                    break;
            }
            lastValueFixed = lastValueFixed.RoundToDecimals(decimalPlaces);
            InvokeValue(lastValueFixed);
        }

        #endregion

        private void InvokeValue(float value)
        {
            valueToFloatEvent?.Invoke(value);
            valueToFloat01Event?.Invoke(Mathf.Clamp01(value));
            valueToIntEvent?.Invoke(Mathf.RoundToInt(value));
            valueToStringEvent.Invoke(lastValueFixed.ToTrimZeroString(decimalPlaces, !IsPercent && haveSeparator));
        }


        private enum EnumOutputValueType
        {
            Percent,
            Percent01,
            Value,
        }
    }
}