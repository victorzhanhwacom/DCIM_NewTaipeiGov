using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DCIMUtils;
using VzDev.DOTweenUtils;
using VzDev.Frameworks.ScrollRectUtils;

public class InRowCooler_ListItem : ScrollRectListItemBase<WebAPI_RealtimeData_InRowCooler>
{
    #region UnityEvents
    [Foldout("[Event]"), SerializeField] private UnityEvent<int> alertLevelStatusEvent;
    [Foldout("[Event]"), SerializeField] private UnityEvent<int> inTempAlertLevelStatusEvent, outTempAlertLevelStatusEvent;
    #endregion

    #region Fields
    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
    [Foldout("[Components]"), SerializeField] private DOTweenText txtInTemp, txtOutTemp;
    #endregion

    protected override void UpdateUI(WebAPI_RealtimeData_InRowCooler data)
    {
        txtDeviceName.SetText(data.deviceName);
        txtInTemp.SetText($"{data.inTempTag.value} {data.inTempTag.unit}");
        txtOutTemp.SetText($"{data.outTempTag.value} {data.outTempTag.unit}");
        alertLevelStatusEvent?.Invoke(data.TotalAlertLevelStatus);
        inTempAlertLevelStatusEvent?.Invoke(data.inTempTag.alertLevelStatus);
        outTempAlertLevelStatusEvent?.Invoke(data.outTempTag.alertLevelStatus);
    }
}