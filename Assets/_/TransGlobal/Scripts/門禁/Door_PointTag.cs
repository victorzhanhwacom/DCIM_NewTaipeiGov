using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DOTweenUtils;

namespace VzDev.DCIMUtils
{
    public class Door_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_Door>
    {
        [Foldout("[Events]"), SerializeField] private UnityEvent<string> valueEvent;
        private string value = null;

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataDoor.WebAPI_DoorData);
            WebAPI_CallerBase_RealtimeDataDoor.OnGetDoorDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataDoor.OnGetDoorDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            if(string.IsNullOrEmpty(value) || value != data.value)
                valueEvent?.Invoke(data.value);
            value = data.value;
        }
    }
}