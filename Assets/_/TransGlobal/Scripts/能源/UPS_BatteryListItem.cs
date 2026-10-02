using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DCIMUtils;
using VzDev.DOTweenUtils;
using VzDev.Frameworks.ScrollRectUtils;

public class UPS_BatteryListItem : ScrollRectListItemBase<WebAPI_RealtimeData_UPSBattery>
{
    #region UnityEvents
    [Foldout("[Events]"), SerializeField] private UnityEvent<int> alertLevelStatusEvent;
    [Foldout("[Event]"), SerializeField] private UnityEvent<int> inVoltageAlertLevelStatusEvent, outIRAlertLevelStatusEvent;
    #endregion

    #region Fields

    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
    [Foldout("[Components]"), SerializeField] private DOTweenText txtVoltage, txtIR;
    #endregion

    protected override void UpdateUI(WebAPI_RealtimeData_UPSBattery data)
    {
        txtDeviceName.SetText(data.deviceName);
        txtVoltage.SetText($"{data.voltageTag.value} {data.voltageTag.unit}");
        txtIR.SetText($"{data.irTag.value} {data.irTag.unit}");
        alertLevelStatusEvent?.Invoke(data.TotalAlertLevelStatus);
        inVoltageAlertLevelStatusEvent?.Invoke(data.voltageTag.alertLevelStatus);
        outIRAlertLevelStatusEvent?.Invoke(data.irTag.alertLevelStatus);
    }
}
