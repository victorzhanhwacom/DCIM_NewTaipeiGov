using VzDev.DCIMUtils;
using VzDev.Frameworks.ScrollRectUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;

public class InRowCooler_List : ScrollRectListBase<WebAPI_RealtimeData_InRowCooler>
{
    override protected void OnEnable()
    {
        base.OnEnable();
        WebAPI_CallerBase_RealtimeDataHVAC.OnGetInRowCoolerDataAction += SetDataList;
    }

    private void OnDisable() => WebAPI_CallerBase_RealtimeDataHVAC.OnGetInRowCoolerDataAction -= SetDataList;

    override protected void OnSelectedItem(ScrollRectListItemBase<WebAPI_RealtimeData_InRowCooler> selectedItem) => ColliderInteractionSystem.SimulateClick(selectedItem.Data.modelInfo.modelTarget.gameObject);
    override protected void OnSelectEmpty() => ColliderInteractionSystem.SimulateClickEmpty();
}
