using System;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.TextUtils
{
    public class TextValueFormatter : MonoBehaviour
    {
        [Foldout("[設定]")]
        public UnityEvent<string> invokeStringValue;
        
        [Foldout("[數值格式]")]
        [SerializeField] private EnumTextFormat textFormat;
        [Foldout("[數值格式]"), ShowIf(nameof(IsOptionF))]
        [SerializeField] private int dotAfterPoint = 2;
        
        
        public void SetValue(float value)
        {
            string format = textFormat.ToString();
            if (textFormat == EnumTextFormat.F) format = "0." + new string('#', dotAfterPoint);
            invokeStringValue?.Invoke(value.ToString(format));
        }
        
        private bool IsOptionF() => textFormat == EnumTextFormat.F;

        public enum EnumTextFormat
        {
            N0, //數字格式（Number，加千分位）	"N0" → 1,235
            F,
            P1,  //百分比格式（Percent）	"P1"（1.2 ➜ 120.0%）
            C0, //貨幣格式（Currency）	"C" → $1,234.56（根據文化區域）
        }
    }
}