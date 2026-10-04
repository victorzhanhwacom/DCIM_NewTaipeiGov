using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

        /// <summary>
        /// 搜尋列表項目 (deviceName)
        /// </summary>
        public void SearchKeyword(string keyword)
        {
            bool haveResult = false;
            int totalFindCount = 0;
            if (string.IsNullOrEmpty(keyword))
            {
                // 若keyword為空，則顯示所有列表項目
                foreach (var item in dataToItemMap.Values)
                {
                    item.gameObject.SetActive(true);
                }
                totalFindCount = dataToItemMap.Count;
                haveResult = true;
                searchTitlePrefix = "";
            }
            else
            {
                // 搜尋deviceName包含keyword的列表項目，並將其顯示出來，其他列表項目隱藏
                foreach (var item in dataToItemMap.Values)
                {
                    bool isMatch = item.Data.deviceName.Contains(keyword, System.StringComparison.OrdinalIgnoreCase);
                    item.gameObject.SetActive(isMatch);
                    if (isMatch) totalFindCount++;
                    if (isMatch || haveResult) haveResult = true;
                }
                searchTitlePrefix = "搜尋結果: ";
            }
            isSearchHaveNoResultEvent?.Invoke(!haveResult);
            listItemTotalCountEvent?.Invoke($"{searchTitlePrefix}共{totalFindCount}筆資料");
        }
        [SerializeField] private UnityEvent<bool> isSearchHaveNoResultEvent;
    }
}
