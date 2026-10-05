using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DCIMUtils.DataUtils;
using VzDev.ObjectUtils;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI即時資料標籤基底類別
    /// </summary>
    public abstract class WebAPI_PointTagBase<TData> : MonoBehaviour where TData : WebAPI_RealtimeData
    {
        #region UnityEvents
        [Foldout("[Event]"), SerializeField, Tooltip("告警等級狀態事件")] private UnityEvent<int> totalAlertLevelStatusEvent;
        #endregion

        #region Fields
        [SerializeField, ReadOnly] protected TData data;
        [Foldout("[Components]"), SerializeField] private UIAnchorFollower uiAnchorFollower;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        protected string deviceCode { get; private set; }
        protected Transform target3DObject => uiAnchorFollower?.Target3DObject;
        #endregion

        protected virtual void Start() => GetDeviceCode();

        /// <summary>
        /// 取得資料後的邏輯處理
        /// </summary>
        protected void OnGetDataAction(List<TData> list)
        {
            if (list == null || list.Count == 0) return;
            GetDeviceCode();
            data = list.FirstOrDefault(data => data.deviceCode == deviceCode);
            if (data == null)
            {
                Debug.LogWarning($"PointTag: 找不到對應的資料, DeviceCode: {deviceCode}");
                return;
            }
            data.modelInfo ??= new ModelInfo();
            data.modelInfo.modelTarget = uiAnchorFollower?.Target3DObject;
            txtDeviceName.SetText(data.deviceName);
            InvokeEvent();
        }

        /// <summary>
        /// 觸發事件
        /// </summary>
        protected virtual void InvokeEvent() => totalAlertLevelStatusEvent?.Invoke(data.TotalAlertLevelStatus);

        /// <summary>
        /// 從UIAnchorFollower的Target3DObject取得DeviceCode
        /// </summary>
        private void GetDeviceCode()
        {
            if (uiAnchorFollower == null) TryGetComponent(out uiAnchorFollower);
            if (string.IsNullOrEmpty(deviceCode) && uiAnchorFollower.Target3DObject != null)
                deviceCode = uiAnchorFollower.Target3DObject.name.GetStringBetweenMarks("[", "]");
        }
    }
}