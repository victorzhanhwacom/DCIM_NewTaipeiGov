using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.String
{
    /// 檢查Text組件的值是否為空值
    public class StringBoolMediator : MonoBehaviour
    {
        [ReadOnly, SerializeField] private bool isHaveValue;
        [Foldout("[Event] 是否有值")] public UnityEvent<bool> isHaveValueEvent, isHaveNonValueEvent;
        [Foldout("[Event] 各別Invoke是否有值")] public UnityEvent onHaveValueEvent, onEmptyValueEvent;

        public void CheckIsNullOrEmpty(TextMeshProUGUI component) => CheckIsNullOrEmpty(component.text);

        public void CheckIsNullOrEmpty(TMP_InputField component) => CheckIsNullOrEmpty(component.text);

        public void CheckIsNullOrEmpty(string txt)
        {
            txt = txt.Trim();
            InvokeEvent(!string.IsNullOrEmpty(txt));
        }

        public void InvokeEvent(bool isOn)
        {
            isHaveValue = isOn;
            isHaveValueEvent.Invoke(isHaveValue);
            isHaveNonValueEvent.Invoke(!isHaveValue);
            (isHaveValue ? onHaveValueEvent : onEmptyValueEvent).Invoke();
        }
    }
}