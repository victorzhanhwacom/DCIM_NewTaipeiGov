using VzDev.DCIMUtils;
using VzDev.Frameworks.ScrollRectUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;

public class FS_List : ScrollRectListBase<WebAPI_RealtimeData_FS>
{
    override protected void OnEnable()
    {
        base.OnEnable();
        WebAPI_CallerBase_RealtimeDataFS.OnGetFSDataAction += SetDataList;
    }

    private void OnDisable() => WebAPI_CallerBase_RealtimeDataFS.OnGetFSDataAction -= SetDataList;

    override protected void OnSelectedItem(ScrollRectListItemBase<WebAPI_RealtimeData_FS> selectedItem) => ColliderInteractionSystem.SimulateClick(selectedItem.Data.modelInfo.modelTarget.gameObject);
    override protected void OnSelectEmpty() => ColliderInteractionSystem.SimulateClickEmpty();
}
