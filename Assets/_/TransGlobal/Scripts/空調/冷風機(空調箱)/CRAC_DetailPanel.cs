using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using VzDev.DCIMUtils;
using VzDev.MediatorUtils;

public class CRAC_DetailPanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private WebAPI_RealtimeData_CRAC data;
    [Foldout("[Components]-AlertLevelStatus"), SerializeField] private ChildrenSwitcher[] alertLevelStatus;
    [Foldout("[Components]-TagDisplayName"), SerializeField] private TextMeshProUGUI[] txtTagDisplayName;
    [Foldout("[Components]-TagValue"), SerializeField] private TextMeshProUGUI[] txtTagValue;
    #endregion

    private void SetData(WebAPI_RealtimeData_CRAC cracData)
    {
        if (cracData == null) return;
        // 在這裡更新面板上的UI元素，顯示cracData的詳細資訊
        UpdateUI();
    }

    private void UpdateUI()
    {
        //將tags的displayName與value分別顯示在txtTagDisplayName與txtTagValue上
        for (int i = 0; i < data.tags.Count; i++)
        {
            alertLevelStatus[i].SetValue(data.tags[i].alertLevelStatus);
            txtTagDisplayName[i].text = data.tags[i].displayName;
            txtTagValue[i].text = data.tags[i].value;
        }
    }

    #region Event Listener
    protected void OnEnable() => WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction += SetDataList;
    private void OnDisable() => WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRACDataAction -= SetDataList;
    private void SetDataList(List<WebAPI_RealtimeData_CRAC> cracData)
    {
        //依照data.deviceCode搜尋cracData，找到對應的WebAPI_RealtimeData_CRAC物件
        var searchResult = cracData.Find(crac => crac.deviceCode == data.deviceCode);
        if (searchResult == null)
        {
            Debug.LogWarning($"CRAC_DetailPanel: No matching data found for deviceCode {data.deviceCode}");
            return;
        }
        SetData(searchResult);
    }
    #endregion


}
