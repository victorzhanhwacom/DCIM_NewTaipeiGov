using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _VictorDev.DebugUtils
{
    /// Bool值處理仲介
    public class BoolValueMediator : MonoBehaviour
    {
        #region Variables

        [Foldout("[Event] 反轉bool值")] public UnityEvent<bool> boolToReverseEvent;
        [Foldout("[Event] 只在Ture時Invoke True")] public UnityEvent<bool> invokeInTrueEvent;
        [Foldout("[Event] 只在False時Invoke True")] public UnityEvent<bool> invokeInFalseEvent;
        [Foldout("[組件]"), SerializeField]private Toggle toggle;

        #endregion

        public void SetValue01(int value)
        {
            value = Mathf.Clamp(value, 0, 1);
            SetBoolValue(value == 1);
        }
        
        public void SetBoolValue(bool value)
        {
            boolToReverseEvent?.Invoke(!value);
            (value? invokeInTrueEvent: invokeInFalseEvent)?.Invoke(true);
        }
        
        private void OnEnable() => toggle?.onValueChanged.AddListener(SetBoolValue);
        private void OnDisable() => toggle?.onValueChanged.RemoveListener(SetBoolValue);
    }
}
