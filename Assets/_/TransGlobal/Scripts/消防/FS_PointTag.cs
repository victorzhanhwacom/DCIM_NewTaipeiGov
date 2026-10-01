using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// 消防區域標籤
    /// </summary>
    public class FS_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_FS>
    {
        #region UnityEvents
        [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> controlBoardValueEvent, levelValue1Event, levelValue2Event, vesdaValueEvent, vesdaDeviceValueEvent;
        [Foldout("[Event]-AlertStatus"), SerializeField] private UnityEvent<int> controlBoardAlertStatusEvent, levelValue1AlertStatusEvent, levelValue2AlertStatusEvent, vesdaAlertStatusEvent, vesdaDeviceAlertStatusEvent;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI titleControlBoard, titleLevel1, titleLevel2, titleVesda, titleVesdaDevice;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataFS.WebAPI_FSData);
            WebAPI_CallerBase_RealtimeDataFS.OnGetFSDataAction += OnGetDataAction;
        }
        private void OnDisable() => WebAPI_CallerBase_RealtimeDataFS.OnGetFSDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            titleControlBoard.SetText(data.controlBoardTag.displayName);
            titleLevel1.SetText(data.level1Tag.displayName);
            titleLevel2.SetText(data.level2Tag.displayName);
            titleVesda.SetText(data.vesdaTag.displayName);
            titleVesdaDevice.SetText(data.vesdaDeviceTag.displayName);

            controlBoardValueEvent?.Invoke(data.controlBoardTag.value);
            levelValue1Event?.Invoke(data.level1Tag.value);
            levelValue2Event?.Invoke(data.level2Tag.value);
            vesdaValueEvent?.Invoke(data.vesdaTag.value);
            vesdaDeviceValueEvent?.Invoke(data.vesdaDeviceTag.value);
        }
    }
}