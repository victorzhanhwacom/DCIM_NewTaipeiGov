namespace VzDev.DCIMUtils
{
    public class Door_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_Door>
    {
        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataDoor.WebAPI_DoorData);
            WebAPI_CallerBase_RealtimeDataDoor.OnGetDoorDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataDoor.OnGetDoorDataAction -= OnGetDataAction;
    }
}