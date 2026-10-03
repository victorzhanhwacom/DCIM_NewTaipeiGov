namespace VzDev.DCIMUtils
{
    public class CCTV_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_CCTV>
    {
        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataCCTV.WebAPI_CCTVData);
            WebAPI_CallerBase_RealtimeDataCCTV.OnGetCCTVDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataCCTV.OnGetCCTVDataAction -= OnGetDataAction;
    }
}