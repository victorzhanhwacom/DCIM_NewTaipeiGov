using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DOTweenUtils;
using VzDev.Frameworks.ScrollRectUtils;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 溫濕度數據綁定器：列表項目
    /// </summary>
    public class RtRhListItem : ScrollRectListItemBase<WebAPI_RealtimeData_RtRh>
    {
        #region UnityEvent
        [Foldout("[Event]"), SerializeField] private UnityEvent<int> totalAlertLevelStatusEvent;
        [Foldout("[Event]"), SerializeField] private UnityEvent<int> rtAlertLevelStatusEvent;
        [Foldout("[Event]"), SerializeField] private UnityEvent<int> rhAlertLevelStatusEvent;
        #endregion
        #region Fields
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        [Foldout("[Components]"), SerializeField] private DOTweenText txtRT, txtRH;
        #endregion

        protected override void UpdateUI(WebAPI_RealtimeData_RtRh data)
        {
            txtDeviceName.SetText(data.deviceName);
            txtRT.SetText($"{data.rtTag.value} {data.rtTag.unit}");
            txtRH.SetText($"{data.rhTag.value} {data.rhTag.unit}");
            totalAlertLevelStatusEvent?.Invoke(data.TotalAlertLevelStatus);
            rtAlertLevelStatusEvent?.Invoke(data.rtTag.alertLevelStatus);
            rhAlertLevelStatusEvent?.Invoke(data.rhTag.alertLevelStatus);
        }
    }
}
