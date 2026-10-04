using UnityEngine;
using UnityEngine.Events;
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

        // <summary>
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
