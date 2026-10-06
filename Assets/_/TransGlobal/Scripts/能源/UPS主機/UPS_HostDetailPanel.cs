using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VzDev.DCIMUtils;

public class UPS_HostDetailPanel : TagDetailPanel
{
    protected override void UpdateUI()
    {
        base.UpdateUI();
        WebAPI_CallerBase_RealtimeDataPower.OnGetUpsHostDataAction += SetDataList;

    }

      #region Event Listener
    protected void OnEnable()
    {
        UpsHost_PointTag.OnSelectedAction += OnSelectedActionHandler;
        UpsHost_PointTag.OnDeselectedAction += OnDeselectedActionHandler;
    }


    private void OnSelectedActionHandler(WebAPI_RealtimeData_UPSHost data, Toggle toggle)
    {
        SetData(data);
        currentToggle = toggle;
    }

    protected void OnDisable()
    {
        UpsHost_PointTag.OnSelectedAction -= OnSelectedActionHandler;
        UpsHost_PointTag.OnDeselectedAction -= OnDeselectedActionHandler;
    }

    private void OnDeselectedActionHandler()
    {
        WebAPI_CallerBase_RealtimeDataPower.OnGetUpsHostDataAction -= SetDataList;
        rootView.SetActive(false);
    }

    private void SetDataList(List<WebAPI_RealtimeData_UPSHost> upsHostData)
    {
        //依照data.deviceCode搜尋upsHostData，找到對應的WebAPI_RealtimeData_UPSHost物件
        var searchResult = upsHostData.Find(upsHost => upsHost.deviceCode == data.deviceCode);
        if (searchResult == null)
        {
            Debug.LogWarning($"TagDetailPanel: No matching data found for deviceCode {data.deviceCode}");
            return;
        }
        SetData(searchResult);
    }
    #endregion
}