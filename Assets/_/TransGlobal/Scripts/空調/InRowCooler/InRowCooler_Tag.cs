using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// InRowCooler標籤
    /// </summary>
    public class InRowCooler_Tag : WebAPI_PointTagBase<WebAPI_RealtimeData_InRowCooler>
    {
        #region UnityEvents
        [Foldout("[Event]"), SerializeField] private UnityEvent<int> inTempAlertLevelStatusEvent, outTempAlertLevelStatusEvent;
        [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> inTempValueEvent, outTempValueEvent;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtInTempTitle, txtOutTempTitle;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataHVAC.WebAPI_InRowCoolerData);
            WebAPI_CallerBase_RealtimeDataHVAC.OnGetInRowCoolerDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataHVAC.OnGetInRowCoolerDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            inTempAlertLevelStatusEvent?.Invoke(data.inTempTag.alertLevelStatus);
            outTempAlertLevelStatusEvent?.Invoke(data.outTempTag.alertLevelStatus);
            txtInTempTitle.SetText(data.inTempTag.displayName);
            txtOutTempTitle.SetText(data.outTempTag.displayName);
            inTempValueEvent?.Invoke($"{data.inTempTag.value} {data.inTempTag.unit}");
            outTempValueEvent?.Invoke($"{data.outTempTag.value} {data.outTempTag.unit}");
        }
    }
}
