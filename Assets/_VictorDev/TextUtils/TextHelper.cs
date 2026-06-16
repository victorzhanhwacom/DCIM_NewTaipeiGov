using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VzDev.DebugUtils;
using VzDev.DoTweenUtils;
using TMPro;
using UnityEngine;

namespace VzDev.TextUtils
{
    public static class TextHelper
    {
        public static void EventCheckIsInputHaveValue(List<TMP_InputField> inputFields, Action<bool> onCheckHandler)
            => inputFields.ForEach(target 
                => target.onValueChanged.AddListener(_=> onCheckHandler?.Invoke(IsInputHaveValue(inputFields))));

        public static bool IsInputHaveValue(List<TMP_InputField> inputFields)
            => inputFields.All(target => string.IsNullOrEmpty(target.text.Trim())) == false;


        #region 依照Text/TextDotween物件名稱與變數名稱，設定內容string
        /// 依照Text/TextDotween物件名稱與變數名稱，設定內容string
        public static void SetParamsToTxtComps<T, TText>(T target, TText[] txtComps, string compHeader = "Txt")
            => SetParamsToTxtComps(target, txtComps.ToList(), compHeader);
        /// 依照Text/TextDotween物件名稱與變數名稱，設定內容string
        public static void SetParamsToTxtComps<T, TText>(T target, List<TText> txtComps, string compHeader = "Txt")
        {
            if (target == null || txtComps == null || txtComps.Count == 0)
            {
                Debug.LogWarning("SetParamsToTxtComps: target 或 txtComps 為空。");
                return;
            }

            var type = target.GetType();
            var textType = typeof(TText);

            var nameProp = textType.GetProperty("name");
            var textProp = textType.GetProperty("text");

            if (nameProp == null || textProp == null)
            {
                Debug.LogError($"SetParamsToTxtComps: {textType.Name} 缺少 name 或 text 屬性。");
                return;
            }

            void ApplyValue(string name, object value)
            {
                var comp = txtComps.FirstOrDefault(c => (string)nameProp.GetValue(c) == name);
                if (comp != null)
                    textProp.SetValue(comp, value?.ToString() ?? string.Empty);
            }

            // --- Fields ---
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                ApplyValue(compHeader + field.Name, field.GetValue(target));

            // --- Properties ---
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || prop.GetGetMethod(false) == null)
                    continue;
                ApplyValue(compHeader + prop.Name, prop.GetValue(target));
            }
        }
        #endregion
    }
}