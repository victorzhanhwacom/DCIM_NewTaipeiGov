using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VzDev.DCIMUtils;
using VzDev.DOTweenUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;

public class UPS_BatteryListItem : MonoBehaviour
{
    #region Fields
    
    [Foldout("[Events]"), SerializeField] private UnityEvent<int> alertLevelEvent;
    [SerializeField, ReadOnly] private WebAPI_RealtimeData_UPSBattery upsbatteryData;
    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
    [Foldout("[Components]"), SerializeField] private DOTweenText txtVoltage, txtIR;
    [Foldout("[Components]"), SerializeField] private Toggle toggle;
    #endregion

    public void SetData(WebAPI_RealtimeData_UPSBattery data)
    {
        upsbatteryData = data;

        if (upsbatteryData == null)
        {
            Debug.LogWarning($"UPS_BatteryListItem: No data found for deviceCode:\n{upsbatteryData.deviceCode}");
            return;
        }

        txtDeviceName.SetText(upsbatteryData.deviceName);
        txtVoltage.SetText($"{upsbatteryData.voltageTag.value} {upsbatteryData.voltageTag.unit}");
        txtIR.SetText($"{upsbatteryData.irTag.value} {upsbatteryData.irTag.unit}");
        alertLevelEvent?.Invoke(upsbatteryData.TotalAlertLevelStatus);
    }

    public void SetToggleGroup(ToggleGroup group) => toggle.group = group;

    private void OnEnable()
    {
        WebAPI_CallerBase_RealtimeDataPower.OnGetUpsBatteryDataAction += OnGetDataAction;
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnDisable()
    {
        WebAPI_CallerBase_RealtimeDataPower.OnGetUpsBatteryDataAction -= OnGetDataAction;
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if(isOn)
        {
            ColliderInteractionSystem.SimulateClick(upsbatteryData.modelInfo.modelTarget.gameObject);
        }else if(toggle.group != null && toggle.group.AnyTogglesOn() == false)
        {
            ColliderInteractionSystem.SimulateClickEmpty();
        }
    }

    private void OnGetDataAction(List<WebAPI_RealtimeData_UPSBattery> list)
    {
        if (list.Count == 0) return;
        upsbatteryData  = list?.FirstOrDefault(data => data.deviceCode == upsbatteryData.deviceCode);
        SetData(upsbatteryData);
    }
}
