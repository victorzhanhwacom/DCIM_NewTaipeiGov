using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DCIMUtils;
using VzDev.DCIMUtils.DataUtils;
using VzDev.DOTweenUtils;
using VzDev.ObjectUtils;
using VzDev.UnityAPI.Extensions;

namespace VzDev
{
    /// <summary>
    /// UPS電池標籤
    /// </summary>
    public class UpsBatteryTag : MonoBehaviour
    {
        #region Fields
        [Foldout("[Events]"), SerializeField] private UnityEvent<int> alertLevelEvent;
        [Foldout("[Data]"), SerializeField, ReadOnly] private WebAPI_RealtimeData_UPSBattery upsbatteryData;
        [Foldout("[Components]"), SerializeField] private UIAnchorFollower uiAnchorFollower;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI titleVoltage, titleIR;
        [Foldout("[Components]"), SerializeField] private DOTweenText txtVoltage, txtIR;
        private string deviceCode;
        #endregion

        private void Start() => CheckDeviceCode();

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataUpsBattery.WebAPI_RawData);
            WebAPI_CallerBase_RealtimeDataUpsBattery.OnGetDataAction += OnGetDataAction;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataUpsBattery.OnGetDataAction -= OnGetDataAction;

        private void OnGetDataAction(List<WebAPI_RealtimeData_UPSBattery> list)
        {
            if (list.Count == 0) return;

            CheckDeviceCode();
            upsbatteryData = list?.FirstOrDefault(data => data.deviceCode == deviceCode);
            if (upsbatteryData == null)
            {
                Debug.LogWarning($"UpsBatteryTag: No data found for deviceCode:\n{deviceCode}");
                return;
            }

            upsbatteryData.modelInfo ??= new ModelInfo();
            upsbatteryData.modelInfo.modelTarget ??= uiAnchorFollower.Target3DObject;
            upsbatteryData.modelInfo.modelName ??= uiAnchorFollower.Target3DObject.name;

            txtDeviceName.SetText(upsbatteryData.deviceName);
            titleVoltage.SetText(upsbatteryData.voltageTag.displayName);
            titleIR.SetText(upsbatteryData.irTag.displayName);

            txtVoltage.SetText($"{upsbatteryData.voltageTag.value} {upsbatteryData.voltageTag.unit}");
            txtIR.SetText($"{upsbatteryData.irTag.value} {upsbatteryData.irTag.unit}");

            alertLevelEvent?.Invoke(upsbatteryData.alertLevel);
        }

        private void CheckDeviceCode()
        {
            if (uiAnchorFollower == null) TryGetComponent(out uiAnchorFollower);
            if (string.IsNullOrEmpty(deviceCode) && uiAnchorFollower.Target3DObject != null)
            {
                deviceCode = uiAnchorFollower.Target3DObject.name.GetStringBetweenMarks("[", "]");
            }
        }
    }
}
