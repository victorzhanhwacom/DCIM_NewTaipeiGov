using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DCIMUtils.EnviornmentUtils;

namespace VzDev.DCIMUtils
{
    public class RtRh_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_RtRh>
    {
        #region UnityEvents
        [Foldout("[Event]"), SerializeField] private UnityEvent<int> rtrhChangeEvent;
        [Foldout("[Event]"), SerializeField] private UnityEvent<int> rtAlertLevelStatusEvent, rhAlertLevelStatusEvent;
        [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> rtValueEvent, rhValueEvent;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataRTRH.WebAPI_RtRhData);
            WebAPI_CallerBase_RealtimeDataRTRH.OnGetRtRhDataAction += OnGetDataAction;

            RtRhDataManager.OnRtRhModeChangedAction += OnRtRhModeChangedAction;
        }

        private void OnRtRhModeChangedAction(RtRhDataManager.EnumRtRhMode mode)
        {
            int result = mode == RtRhDataManager.EnumRtRhMode.Rt ? 0 : mode == RtRhDataManager.EnumRtRhMode.Rh ? 1 : -1;
            if (result == -1)
            {
                Debug.LogWarning($"[RtRh_PointTag] RtRhDataManager.EnumRtRhMode is {mode}, but RtRh_PointTag only support Rt or Rh mode.");
                return;
            }
            rtrhChangeEvent?.Invoke(result);
        }

        private void OnDisable()
        {
            WebAPI_CallerBase_RealtimeDataRTRH.OnGetRtRhDataAction -= OnGetDataAction;
            RtRhDataManager.OnRtRhModeChangedAction -= OnRtRhModeChangedAction;
        }

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            rtAlertLevelStatusEvent?.Invoke(data.rtTag.alertLevelStatus);
            rhAlertLevelStatusEvent?.Invoke(data.rhTag.alertLevelStatus);
            rtValueEvent?.Invoke($"{data.rtTag.value} {data.rtTag.unit}");
            rhValueEvent?.Invoke($"{data.rhTag.value} {data.rhTag.unit}");
        }
    }
}