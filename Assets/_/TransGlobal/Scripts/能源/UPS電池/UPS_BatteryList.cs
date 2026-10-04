using System.Collections.Generic;
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

        public override void SetDataList(List<WebAPI_RealtimeData_UPSBattery> dataList)
        {
            base.SetDataList(dataList);

            // 在dataToItemMap裡依據TotalAlertLevelStatus排序，將有告警的設備排在前面，其次再進行deviceName的排序，並設置ScrollRectListItemBase的SiblingIndex
            var sortedItems = new List<ScrollRectListItemBase<WebAPI_RealtimeData_UPSBattery>>(dataToItemMap.Values);
            sortedItems.Sort((a, b) =>
            {
                int alertComparison = b.Data.TotalAlertLevelStatus.CompareTo(a.Data.TotalAlertLevelStatus);
                if (alertComparison != 0) return alertComparison;
                return a.Data.deviceName.CompareTo(b.Data.deviceName);
            }); 

            for (int i = 0; i < sortedItems.Count; i++)
            {
                sortedItems[i].transform.SetSiblingIndex(i);
            }   
        }
    }
}
