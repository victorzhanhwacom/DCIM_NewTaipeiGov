using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static VzDev.DCIMUtils.WebAPI_RealtimeData;

namespace VzDev.DCIMUtils
{
    public class UpsHost_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_UPSHost>
    {
        #region Events
        public static Action<WebAPI_RealtimeData_UPSHost, Toggle> OnSelectedAction;
        public static Action OnDeselectedAction;

        [Foldout("[Events]-Tag"), SerializeField] private UnityEvent<Tag> outputTotalWattTagEvent;
        [Foldout("[Events]-Tag"), SerializeField] private UnityEvent<Tag> batteryModeTagEvent;
        [Foldout("[Events]-AlertLevelStatus"), SerializeField] private UnityEvent<int> outputTotalWattAlertLevelStatusEvent;
        [Foldout("[Events]-Value"), SerializeField] private UnityEvent<string> outputTotalWattValueEvent;

        #endregion

        #region Fields
        [Foldout("[Components]"), SerializeField] private Toggle toggle;
        #endregion

        protected override void InvokeEvent()
        {
            base.InvokeEvent();
            outputTotalWattTagEvent?.Invoke(data.OutputTotalWattTag);
            batteryModeTagEvent?.Invoke(data.BatteryModeTag);

            outputTotalWattAlertLevelStatusEvent?.Invoke(data.OutputTotalWattTag?.alertLevelStatus ?? 0);
            outputTotalWattValueEvent?.Invoke(data.OutputTotalWattTag?.valueWithUnit);
        }

        public void ToSelected(bool isOn)
        {
            if (isOn) OnSelectedAction?.Invoke(data, toggle);
            else OnDeselectedAction?.Invoke();
        }

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataPower.WebAPI_UpsHostData);
            WebAPI_CallerBase_RealtimeDataPower.OnGetUpsHostDataAction += OnGetDataAction;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataPower.OnGetUpsHostDataAction -= OnGetDataAction;
    }
}