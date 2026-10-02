using VzDev.DCIMUtils;
using VzDev.Frameworks.ScrollRectUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;

public class CRAC_List : ScrollRectListBase<WebAPI_RealtimeData_CRAC>
{
    override protected void OnEnable()
    {
        base.OnEnable();
        WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction += SetDataList;
    }

    private void OnDisable() => WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction -= SetDataList;
    
    override protected void OnSelectedItem(ScrollRectListItemBase<WebAPI_RealtimeData_CRAC> selectedItem) => ColliderInteractionSystem.SimulateClick(selectedItem.Data.modelInfo.modelTarget.gameObject);
    override protected void OnSelectEmpty() => ColliderInteractionSystem.SimulateClickEmpty();
}
