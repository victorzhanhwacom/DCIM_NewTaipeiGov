using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    public class RtRhDataManager : MonoBehaviour
    {
        public enum EnumRtRhMode
        {
            Unselect,
            Rt,
            Rh,
            WaterLeak
        }

        #region Events
        [Foldout("[Events]-THS")] public UnityEvent<bool> onRtModeEvent, onRhModeEvent;
        [Foldout("[Events]-WLK")] public UnityEvent<bool> onWaterLeakModeEvent;
        #endregion

        #region Fields
        [SerializeField, OnValueChanged("OnCurrentModeChanged")] private EnumRtRhMode currentMode = EnumRtRhMode.Unselect;
        private void OnCurrentModeChanged()
        {
            ToRtMode(currentMode == EnumRtRhMode.Rt);
            ToRhMode(currentMode == EnumRtRhMode.Rh);
            ToWaterLeakMode(currentMode == EnumRtRhMode.WaterLeak);
        }
        #endregion

        public void InvokeStatus() => OnCurrentModeChanged();

        public void ToRtMode(bool isOn)
        {
            onRtModeEvent?.Invoke(isOn);
            if (isOn == false) return;
            currentMode = EnumRtRhMode.Rt;
            OnRtRhModeChangedAction?.Invoke(currentMode);
            ToWaterLeakMode(false);
        }

        public void ToRhMode(bool isOn)
        {
            onRhModeEvent?.Invoke(isOn);
            if (isOn == false) return;
            currentMode = EnumRtRhMode.Rh;
            OnRtRhModeChangedAction?.Invoke(currentMode);
            ToWaterLeakMode(false);
        }

        public void ToWaterLeakMode(bool isOn)
        {
            onWaterLeakModeEvent?.Invoke(isOn);
            if (isOn == false) return;
            currentMode = EnumRtRhMode.WaterLeak;
        }

        #region Static Actions
        public static Action<EnumRtRhMode> OnRtRhModeChangedAction;
        #endregion
    }
}
