using VzDev.Frameworks.ScrollRectUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;
namespace VzDev.DCIMUtils
{
    /// <summary>
    /// 漏水帶數據綁定器：列表
    /// </summary>
    public class WaterLeakList : ScrollRectListBase<WebAPI_RealtimeData_WaterLeak>
    {

        override protected void OnEnable()
        {
            base.OnEnable();
            WebAPI_CallerBase_RealtimeDataWaterLeak.OnGetWaterLeakDataAction += SetDataList;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataWaterLeak.OnGetWaterLeakDataAction -= SetDataList;

        override protected void OnSelectedItem(ScrollRectListItemBase<WebAPI_RealtimeData_WaterLeak> selectedItem) => ColliderInteractionSystem.SimulateClick(selectedItem.Data.modelInfo.modelTarget.gameObject);
        override protected void OnSelectEmpty() => ColliderInteractionSystem.SimulateClickEmpty();
    }
}
