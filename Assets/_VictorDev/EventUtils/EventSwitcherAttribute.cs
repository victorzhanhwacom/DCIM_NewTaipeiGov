using System;
using UnityEngine.Events;

namespace _VictorDev.EventUtils
{
    /// 處理Toggle.IsOn狀態事件Invoke
    [Serializable]
    public struct EventSwitcherAttribute
    {
        public UnityEvent<bool> isSwitched;
        public UnityEvent<bool> isSwitchOn;
        public UnityEvent<bool> isSwitchOff;

        public void SetSwitch(bool isOn)
        {
            isSwitched?.Invoke(isOn);
            if (isOn) isSwitchOn?.Invoke(true);
            else isSwitchOff?.Invoke(false);
        }
    }
}