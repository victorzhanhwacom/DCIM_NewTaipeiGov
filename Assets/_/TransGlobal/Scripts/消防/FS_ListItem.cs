using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.Frameworks.ScrollRectUtils;

namespace VzDev.DCIMUtils
{
    public class FS_ListItem : ScrollRectListItemBase<WebAPI_RealtimeData_FS>
    {
        #region UnityEvents
        [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> controlBoardValueEvent, levelValue1Event, levelValue2Event, vesdaValueEvent, vesdaDeviceValueEvent;
        [Foldout("[Event]-AlertStatus"), SerializeField] private UnityEvent<int> controlBoardAlertStatusEvent, levelValue1AlertStatusEvent, levelValue2AlertStatusEvent, vesdaAlertStatusEvent, vesdaDeviceAlertStatusEvent;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtControlBoard, txtLevel1, txtLevel2, txtVesda, txtVesdaDevice;
        #endregion


        #region Fields
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        #endregion

        protected override void UpdateUI(WebAPI_RealtimeData_FS data)
        {
            txtDeviceName.SetText(data.deviceName);
            controlBoardValueEvent?.Invoke(data.controlBoardTag.value);
            levelValue1Event?.Invoke(data.level1Tag.value);
            levelValue2Event?.Invoke(data.level2Tag.value);
            vesdaValueEvent?.Invoke(data.vesdaTag.value);
            vesdaDeviceValueEvent?.Invoke(data.vesdaDeviceTag.value);
        }
    }
}
