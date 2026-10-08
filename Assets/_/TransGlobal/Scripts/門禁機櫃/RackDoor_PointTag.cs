using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DCIMUtils
{
    public class RackDoor_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_RackDeviceCode>
    {
        public UnityEvent<string> doorNameEvent;
        public UnityEvent<int> isDoorOpenFrontEvent, isDoorOpenBackEvent;
        public UnityEvent<string> frontdoorStatusValueEvent, backdoorStatusValueEvent;
        public List<DetailListItem> detailListItemsFrontDoor;
        public List<DetailListItem> detailListItemsBackDoor;

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataDoor.WebAPI_RackDeviceCodeData);
            WebAPI_CallerBase_RealtimeDataDoor.OnGetRackDeviceCodeDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataDoor.OnGetRackDeviceCodeDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            if(data.FrontDoorData == null || data.BackDoorData == null) return;

            string doorName = data.FrontDoorData.DisplayName;
            doorNameEvent?.Invoke(doorName);

            detailListItemsFrontDoor[0].SetData(data.FrontDoorData.ConnectStatusTag);
            detailListItemsBackDoor[0].SetData(data.BackDoorData.ConnectStatusTag);
            detailListItemsFrontDoor[1].SetData(data.FrontDoorData.LockStatusTag);
            detailListItemsBackDoor[1].SetData(data.BackDoorData.LockStatusTag);
            detailListItemsFrontDoor[2].SetData(data.FrontDoorData.doorStatusTag);
            detailListItemsBackDoor[2].SetData(data.BackDoorData.doorStatusTag);
            detailListItemsFrontDoor[3].SetData(data.FrontDoorData.doorHandlerTag);
            detailListItemsBackDoor[3].SetData(data.BackDoorData.doorHandlerTag);
            detailListItemsFrontDoor[4].SetData(data.FrontDoorData.lastCardTag);
            detailListItemsBackDoor[4].SetData(data.BackDoorData.lastCardTag);

            isDoorOpenFrontEvent?.Invoke(data.FrontDoorData.isDoorOpen ? 1 : 0);
            isDoorOpenBackEvent?.Invoke(data.BackDoorData.isDoorOpen ? 1 : 0);

            frontdoorStatusValueEvent?.Invoke(data.FrontDoorData.isDoorOpen ? "開門" : "關門");
            backdoorStatusValueEvent?.Invoke(data.BackDoorData.isDoorOpen ? "開門" : "關門");
        }
    }
}