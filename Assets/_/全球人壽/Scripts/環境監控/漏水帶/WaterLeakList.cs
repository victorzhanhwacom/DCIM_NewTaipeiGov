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
        [Foldout("[Events]")] public UnityEvent<RealtimeAsset_RtRh> onListItemSelectedEvent;
        #endregion
        #region Fields
        [Foldout("[Data]"), SerializeField, ReadOnly] private List<RealtimeAsset_RtRh> rtrhData;
        [Foldout("[Component]"), SerializeField] private RtRhListItem listItemPrefab;
        [Foldout("[Component]"), SerializeField] private ScrollRect scRtRhList, scSearchList;
        [Foldout("[Component]"), SerializeField] private ToggleGroup tgRtRhList, tgSearchList;
        [Foldout("[Component]"), SerializeField] private TextMeshProUGUI txtRtRhDataCount, txtSearchResultCount;

        private List<RtRhListItem> listItems = new List<RtRhListItem>();
        #endregion

        private void Awake()
        {
            WebApiRealtimeDataHandler_RtRh.OnGetRealtimeAssetAction += OnGetRealtimeAssetAction;
            OnGetRealtimeAssetAction(WebApiRealtimeDataHandler_RtRh.RealtimeAssets);
        }

        private void OnGetRealtimeAssetAction(RealtimeAsset_RtRh[] data)
        {
            rtrhData = new List<RealtimeAsset_RtRh>(data);
            txtRtRhDataCount.SetText($"共 {rtrhData.Count} 筆資料");

            ///檢查目前的listItems中是否已經有相同的deviceCode，若沒有則移除該listItem
            listItems.RemoveAll(item => rtrhData.Exists(data => data.deviceCode == item.RtRhData.deviceCode) == false);

            rtrhData.ForEach(data =>
            {
                ///檢查目前的listItems中是否已經有相同的deviceCode，若有則不再新增，直接更新該listItem的資料
                RtRhListItem existingItem = listItems.Find(item => item.RtRhData.deviceCode == data.deviceCode);
                if (existingItem != null)
                {
                    existingItem.SetRtRhData(data);
                    return;
                }

                RtRhListItem listItem = Instantiate(listItemPrefab, scRtRhList.content);
                listItem.name += $"-{data.deviceCode}";
                listItem.SetRtRhData(data);
                listItem.SetToggleGroup(tgRtRhList);
                listItems.Add(listItem);
            });
        }

        private void ClearListItem()
        {
            scRtRhList.content.RemoveAllChildren();
            scRtRhList.verticalNormalizedPosition = 1f;
        }

        public void Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return;
            ClearSearchResult();

            //先以deviceName搜尋，若找不到，再以資產編號搜尋
            var result = rtrhData.FindAll(item => item.deviceName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            if (result.Count == 0) result = rtrhData.FindAll(item => 
                string.IsNullOrEmpty(item.companyAssetInfo.assetNumber) == false &&
                item.companyAssetInfo.assetNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            txtSearchResultCount.SetText($"搜尋結果：共 {result.Count} 筆資料");
            result.ForEach(item =>
            {
                RtRhListItem listItem = Instantiate(listItemPrefab, scSearchList.content);
                listItem.name += $"-{item.deviceCode}";
                listItem.SetToggleGroup(tgSearchList);
                listItem.SetRtRhData(item);
            });
        }

        public void ClearSearchResult()
        {
            scSearchList.content.RemoveAllChildren();
            scSearchList.verticalNormalizedPosition = 1f;
        }
    }
}
