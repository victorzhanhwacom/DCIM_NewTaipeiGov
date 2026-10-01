using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DCIMUtils
{
    public class WaterLeak_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_WaterLeak>
    {
        #region UnityEvents
        [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> titleEvent;
        [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> valueEvent;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataWaterLeak.WebAPI_WaterLeakData);
            WebAPI_CallerBase_RealtimeDataWaterLeak.OnGetWaterLeakDataAction += OnGetDataAction;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataWaterLeak.OnGetWaterLeakDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            titleEvent?.Invoke(data.title);
            valueEvent?.Invoke(data.value);
        }
    }
}