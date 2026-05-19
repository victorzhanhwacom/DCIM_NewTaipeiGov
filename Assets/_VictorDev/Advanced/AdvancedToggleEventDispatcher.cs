using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _VictorDev.Advanced
{
    /// 在Inspector裡進階處理Toggle.OnValueChange事件
    /// <para>+ 直接掛在GameObject上即可</para>
    public class AdvancedToggleEventDispatcher : MonoBehaviour
    {
        private void Start()
        {
            if (isInvokeInStart) ToggleInstance.onValueChanged.Invoke(ToggleInstance.isOn);
            
            ToggleInstance.onValueChanged.AddListener(
                (isOn) =>
                {
                    if (isOn) onValueToTrue?.Invoke();
                    else onValueToFalse?.Invoke();

                    onValueToReverse?.Invoke(!isOn);
                });
        }

        #region Variables

        [Foldout("[Event] - Toggle值反向Invoke")] public UnityEvent<bool> onValueToReverse;

        [Foldout("[Event] - On")] [Header(">>> 當Toggle值為True時")]
        public UnityEvent onValueToTrue;

        [Foldout("[Event] - Off")] [Header(">>> 當Toggle值為False時")]
        public UnityEvent onValueToFalse;

        [Foldout(">>> Start時自動Invoke")] public bool isInvokeInStart = true;

        private Toggle ToggleInstance => _toggle ??= GetComponent<Toggle>();
        [NonSerialized] private Toggle _toggle;

        #endregion
    }
}