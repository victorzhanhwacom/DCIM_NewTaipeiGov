namespace VzDev.DCIMUtils
{
    public class RackDoor_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_RackDoor>
    {
        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataDoor.WebAPI_RackDoorData);
            WebAPI_CallerBase_RealtimeDataDoor.OnGetRackDoorDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataDoor.OnGetRackDoorDataAction -= OnGetDataAction;
    }
}