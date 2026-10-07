namespace VzDev.DCIMUtils
{
    public class RackDoor_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_RackDeviceCode>
    {
        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataDoor.WebAPI_RackDeviceCodeData);
            WebAPI_CallerBase_RealtimeDataDoor.OnGetRackDeviceCodeDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataDoor.OnGetRackDeviceCodeDataAction -= OnGetDataAction;
    }
}