using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DCIMUtils
{
    public class CRAC_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_CRAC>
    {
        #region UnityEvents
        [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> rtValueEvent, tempSetValueEvent;
        [Foldout("[Event]-手動狀態"), SerializeField] private UnityEvent<bool> powerStatusEvent;
        [Foldout("[Event]-AlertStatus"), SerializeField] private UnityEvent<int> rtAlertStatusEvent;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataHVAC.WebAPI_CRACData);
            WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction += OnGetDataAction;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            rtValueEvent?.Invoke($"{data.rtTag.value} {data.rtTag.unit}");
            tempSetValueEvent?.Invoke($"{data.tempSetTag.value} {data.tempSetTag.unit}");
            powerStatusEvent?.Invoke(data.powerStatusTag.value == "開機");
            rtAlertStatusEvent?.Invoke(data.rtTag.alertLevel);
        }
    }
}