using VzDev.Frameworks.ScrollRectUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;
namespace VzDev.DCIMUtils
{
    /// <summary>
    /// 溫濕度數據綁定器：列表
    /// </summary>
    public class RtRhList : ScrollRectListBase<WebAPI_RealtimeData_RtRh>
    {
        override protected void OnEnable()
        {
            base.OnEnable();
            WebAPI_CallerBase_RealtimeDataRTRH.OnGetRtRhDataAction += SetDataList;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataRTRH.OnGetRtRhDataAction -= SetDataList;

        override protected void OnSelectedItem(ScrollRectListItemBase<WebAPI_RealtimeData_RtRh> selectedItem) => ColliderInteractionSystem.SimulateClick(selectedItem.Data.modelInfo.modelTarget.gameObject);
        override protected void OnSelectEmpty() => ColliderInteractionSystem.SimulateClickEmpty();
    }
}

