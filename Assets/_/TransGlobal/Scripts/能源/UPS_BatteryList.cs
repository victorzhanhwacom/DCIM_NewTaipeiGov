using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using VzDev.ApiExtensions;
using VzDev.DCIMUtils;
using VzDev.DOTweenUtils;

public class UPS_BatteryList : MonoBehaviour
{
    #region Fields
    [Foldout("[Components]"), SerializeField] private UPS_BatteryListItem listItemPrefab;
    [Foldout("[Components]"), SerializeField] private DOTweenText txtTotalCount, txtTotalCountSearch;
    [Foldout("[Components]"), SerializeField] private ScrollRect scrollRect, scrollRectSearch;
    [Foldout("[Components]"), SerializeField] private ToggleGroup toggleGroup, toggleGroupSearch;

    private List<WebAPI_RealtimeData_UPSBattery> upsbatteryDataList;
    private List<WebAPI_RealtimeData_UPSBattery> filteredDataList;
    #endregion

    private void OnEnable()
    {
        scrollRect.content.RemoveAllChildren();
        scrollRectSearch.content.RemoveAllChildren();
        OnGetDataAction(WebAPI_CallerBase_RealtimeDataPower.WebAPI_UpsBatteryData);
        WebAPI_CallerBase_RealtimeDataPower.OnGetUpsBatteryDataAction += OnGetDataAction;
    }

    private void OnDisable() => WebAPI_CallerBase_RealtimeDataPower.OnGetUpsBatteryDataAction -= OnGetDataAction;

    private void OnGetDataAction(List<WebAPI_RealtimeData_UPSBattery> list)
    {
        upsbatteryDataList = list;
        txtTotalCount.SetText($"共{upsbatteryDataList.Count}筆");
        if (upsbatteryDataList.Count == 0) return;

        upsbatteryDataList.ForEach(data =>
        {
            var listItem = Instantiate(listItemPrefab, scrollRect.content);
            listItem.SetData(data);
            listItem.SetToggleGroup(toggleGroup);
        });
    }
}
