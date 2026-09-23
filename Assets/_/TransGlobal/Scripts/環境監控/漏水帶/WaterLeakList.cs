using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VzDev.ApiExtensions;
namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 漏水帶數據綁定器：列表
    /// </summary>
    public class WaterLeakList : MonoBehaviour
    {

        #region UnityEvents
        [Foldout("[Events]")] public UnityEvent<RealtimeAsset_WaterLeak> onListItemSelectedEvent;
        #endregion
        #region Fields
        [Foldout("[Data]"), SerializeField, ReadOnly] private List<RealtimeAsset_WaterLeak> waterleakData;
        [Foldout("[Component]"), SerializeField] private WaterLeakListItem listItemPrefab;
        [Foldout("[Component]"), SerializeField] private ScrollRect scWaterLeakList, scSearchList;
        [Foldout("[Component]"), SerializeField] private ToggleGroup tgWaterLeakList, tgSearchList;
        [Foldout("[Component]"), SerializeField] private TextMeshProUGUI txtWaterLeakDataCount, txtSearchResultCount;

        private List<WaterLeakListItem> listItems = new List<WaterLeakListItem>();
        #endregion

        private void Awake()
        {
            WebApiRealtimeDataHandler_WLK.OnGetRealtimeAssetAction += OnGetRealtimeAssetAction;
            OnGetRealtimeAssetAction(WebApiRealtimeDataHandler_WLK.RealtimeAssets);
        }

        private void OnGetRealtimeAssetAction(List<RealtimeAsset_WaterLeak> data)
        {
            waterleakData = new List<RealtimeAsset_WaterLeak>(data);
            txtWaterLeakDataCount.SetText($"共 {waterleakData.Count} 筆資料");

            ///檢查目前的listItems中是否已經有相同的deviceCode，若沒有則移除該listItem
            listItems.RemoveAll(item => waterleakData.Exists(data => data.deviceCode == item.WaterLeakData.deviceCode) == false);

            waterleakData.ForEach(data =>
            {
                ///檢查目前的listItems中是否已經有相同的deviceCode，若有則不再新增，直接更新該listItem的資料
                WaterLeakListItem existingItem = listItems.Find(item => item.WaterLeakData.deviceCode == data.deviceCode);
                if (existingItem != null)
                {
                    existingItem.SetWaterLeakData(data);
                    return;
                }

                WaterLeakListItem listItem = Instantiate(listItemPrefab, scWaterLeakList.content);
                listItem.name += $"-{data.deviceCode}";
                listItem.SetWaterLeakData(data);
                listItem.SetToggleGroup(tgWaterLeakList);
                listItems.Add(listItem);
            });
        }

        private void ClearListItem()
        {
            scWaterLeakList.content.RemoveAllChildren();
            scWaterLeakList.verticalNormalizedPosition = 1f;
        }

        public void Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return;
            ClearSearchResult();

            //先以deviceName搜尋，若找不到，再以資產編號搜尋
            var result = waterleakData.FindAll(item => item.deviceName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            if (result.Count == 0) result = waterleakData.FindAll(item => 
                string.IsNullOrEmpty(item.companyAssetInfo.assetNumber) == false &&
                item.companyAssetInfo.assetNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            txtSearchResultCount.SetText($"搜尋結果：共 {result.Count} 筆資料");
            result.ForEach(item =>
            {
                WaterLeakListItem listItem = Instantiate(listItemPrefab, scSearchList.content);
                listItem.name += $"-{item.deviceCode}";
                listItem.SetToggleGroup(tgSearchList);
                listItem.SetWaterLeakData(item);
            });
        }

        public void ClearSearchResult()
        {
            scSearchList.content.RemoveAllChildren();
            scSearchList.verticalNormalizedPosition = 1f;
        }
    }
}
