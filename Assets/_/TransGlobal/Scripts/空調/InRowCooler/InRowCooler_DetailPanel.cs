using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VzDev.DCIMUtils;

public class InRowCooler_DetailPanel : MonoBehaviour
{
    #region Fields
    [SerializeField, ReadOnly] private WebAPI_RealtimeData_InRowCooler data;
    [Foldout("[Components]"), SerializeField] private ScrollRect scrollRect;
    [Foldout("[Components]"), SerializeField] private GameObject rootView;
    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
    [Foldout("[Components]-ListItem"), SerializeField] private DetailListItem[] detailListItems;
    private Toggle currentToggle;

    #endregion

    private void SetData(WebAPI_RealtimeData_InRowCooler inRowCoolerData)
    {
        if (inRowCoolerData == null) return;
        data = inRowCoolerData;
        // 在這裡更新面板上的UI元素，顯示inRowCoolerData的詳細資訊
        UpdateUI();
    }

    private void UpdateUI()
    {
        txtDeviceName.SetText(data.deviceName);
        scrollRect.verticalNormalizedPosition = 1f; // 將ScrollRect的垂直位置設置為頂部

        string value;

        //將tags的displayName與value分別顯示在txtTagDisplayName與txtTagValue上
        for (int i = 0; i < data.tags.Count; i++)
        {
            DetailListItem detailListItem = detailListItems[i];
            if (detailListItem == null)
            {
                Debug.LogWarning($"InRowCooler_DetailPanel: detailListItem at index {i} is null.\nDisplayName: {data.tags[i].displayName}, Value: {data.tags[i].value}, AlertLevelStatus: {data.tags[i].alertLevelStatus}");
                continue;
            }

            value = data.tags[i].value;
            if (data.tags[i].unit != null && data.tags[i].unit != "")
            {
                value += $" {data.tags[i].unit}";
            }

            detailListItem.SetData(data.tags[i].displayName, value, data.tags[i].alertLevelStatus);
        }
        WebAPI_CallerBase_RealtimeDataHVAC.OnGetInRowCoolerDataAction += SetDataList;
        rootView.SetActive(true); // 確保面板在更新後是可見的
    }

    public void ToHide()
    {
        if (currentToggle != null)
            currentToggle.isOn = false;
    }

    #region Event Listener
    protected void OnEnable()
    {
        InRowCooler_Tag.OnSelectInRowCoolerDataAction += OnSelectInRowCoolerDataActionHandler;
        InRowCooler_Tag.OnDeselectInRowCoolerAction += OnDeselectInRowCoolerActionHandler;
    }


    private void OnSelectInRowCoolerDataActionHandler(WebAPI_RealtimeData_InRowCooler data, Toggle toggle)
    {
        SetData(data);
        currentToggle = toggle;
    }

    protected void OnDisable()
    {
        InRowCooler_Tag.OnSelectInRowCoolerDataAction -= OnSelectInRowCoolerDataActionHandler;
        InRowCooler_Tag.OnDeselectInRowCoolerAction -= OnDeselectInRowCoolerActionHandler;
    }

    private void OnDeselectInRowCoolerActionHandler()
    {
        WebAPI_CallerBase_RealtimeDataHVAC.OnGetInRowCoolerDataAction -= SetDataList;
        rootView.SetActive(false);
    }

    private void SetDataList(List<WebAPI_RealtimeData_InRowCooler> inRowCoolerData)
    {
        //依照data.deviceCode搜尋inRowCoolerData，找到對應的WebAPI_RealtimeData_InRowCooler物件
        var searchResult = inRowCoolerData.Find(inRowCooler => inRowCooler.deviceCode == data.deviceCode);
        if (searchResult == null)
        {
            Debug.LogWarning($"InRowCooler_DetailPanel: No matching data found for deviceCode {data.deviceCode}");
            return;
        }
        SetData(searchResult);
    }
    #endregion


}
