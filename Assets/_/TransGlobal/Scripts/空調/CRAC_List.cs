using VzDev.DCIMUtils;
using VzDev.Frameworks.ScrollRectUtils;

public class CRAC_List : ScrollRectListBase<WebAPI_RealtimeData_CRAC>
{
    override protected void OnEnable()
    {
        base.OnEnable();
        WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction += SetDataList;
    }

    private void OnDisable() => WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction -= SetDataList;
}
