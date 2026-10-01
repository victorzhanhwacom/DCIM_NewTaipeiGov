using VzDev.Frameworks.ScrollRectUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;

namespace VzDev.DCIMUtils
{
    public class UPS_BatteryList : ScrollRectListBase<WebAPI_RealtimeData_UPSBattery>
    {

        override protected void OnEnable()
        {
            base.OnEnable();
            WebAPI_CallerBase_RealtimeDataPower.OnGetUpsBatteryDataAction += SetDataList;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataPower.OnGetUpsBatteryDataAction -= SetDataList;

        override protected void OnSelectedItem(ScrollRectListItemBase<WebAPI_RealtimeData_UPSBattery> selectedItem) => ColliderInteractionSystem.SimulateClick(selectedItem.Data.modelInfo.modelTarget.gameObject);
        override protected void OnSelectEmpty() => ColliderInteractionSystem.SimulateClickEmpty();
    }
}
