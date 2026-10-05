using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// 消防鋼瓶標籤
    /// </summary>
    public class GasCylinder_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_GasCylinder>
    {
        #region Event
        [Foldout("[Event]"), SerializeField] private UnityEvent<string> displayNameEvent;
        [Foldout("[Event]"), SerializeField] private UnityEvent<string> messageEvent;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataFS.WebAPI_GasCylinderData);
            WebAPI_CallerBase_RealtimeDataFS.OnGetGasCylinderDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataFS.OnGetGasCylinderDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            displayNameEvent?.Invoke(data.displayName);
            messageEvent?.Invoke(data.message);
        }
    }
}