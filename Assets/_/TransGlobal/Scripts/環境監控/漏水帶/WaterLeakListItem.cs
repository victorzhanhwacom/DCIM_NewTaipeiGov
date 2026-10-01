using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DOTweenUtils;
using VzDev.Frameworks.ScrollRectUtils;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 漏水帶數據綁定器：列表項目
    /// </summary>
    public class WaterLeakListItem : ScrollRectListItemBase<WebAPI_RealtimeData_WaterLeak>
    {
        #region UnityEvent
        [Foldout("[Event]"), SerializeField] private UnityEvent<int> totalAlertLevelStatusEvent;
        #endregion
        #region Fields
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        [Foldout("[Components]"), SerializeField] private DOTweenText txtValue;
        #endregion

        protected override void UpdateUI(WebAPI_RealtimeData_WaterLeak data)
        {
            txtDeviceName.SetText(data.deviceName);
            txtValue.SetText(data.value);
            totalAlertLevelStatusEvent?.Invoke(data.TotalAlertLevelStatus);
        }
    }
}
