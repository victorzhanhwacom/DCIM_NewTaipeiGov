using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VzDev.DCIMUtils.Extensions;
using VzDev.UnityAPI.Extensions;
using static VzDev.DCIMUtils.EnviornmentUtils.RtRhDataManager;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 溫濕度數據綁定器：模型
    /// </summary>
    public class DataModelBinder_RTRH : MonoBehaviour
    {
        #region Events
        public Action<RealtimeAsset_RtRh> OnRtRhDataChangedAction;
        public Action<EnumRtRhMode> OnRtRhTypeChangedAction;
        #endregion

        #region Fields
        [SerializeField, ReadOnly] private EnumRtRhMode rtRhMode = EnumRtRhMode.Unselect;
        [SerializeField, ReadOnly] private RealtimeAsset_RtRh rtrhData;
        [Foldout("[Components]"), SerializeField] private HeatSource heatSource;

        public RealtimeAsset_RtRh RtRhData => rtrhData;
        public EnumRtRhMode RtRhMode => rtRhMode;
        #endregion

        private void Awake()
        {
            if (transform.TryGetComponentAndLog(out heatSource) == false)
            {
                Debug.LogWarning($"DataModelBinder_RTRH: HeatSource component not found on the GameObject.", this);
            }
        }

        /// <summary>
        /// 更新HeatSource的溫度值 與 Invoke Event
        /// </summary>
        private void UpdateHeatSource()
        {
            if (heatSource == null && transform.TryGetComponentAndLog(out heatSource) == false) return;
            switch (rtRhMode)
            {
                case EnumRtRhMode.Rt:
                    heatSource.SetTemperature(float.Parse(rtrhData.rtTag.value));
                    break;
                case EnumRtRhMode.Rh:
                    heatSource.SetTemperature(float.Parse(rtrhData.rhTag.value));
                    break;
            }
        }

        /// <summary>
        /// WebAPI回傳即時資料時
        /// </summary>
        private void OnGetRealtimeAssetAction(RealtimeAsset_RtRh[] list)
        {
            string deviceCode = transform.parent.GetModelDeviceCode();
            rtrhData = list.FirstOrDefault(data => data.deviceCode == deviceCode);
            if (rtrhData == null)
            {
                Debug.LogWarning($"DataModelBinder_RTRH: No matching data found for deviceCode in the list.", this);
                return;
            }
            OnRtRhDataChangedAction?.Invoke(rtrhData);
            UpdateHeatSource();
        }

        /// <summary>
        /// 改變溫濕度模式時
        /// </summary>
        private void OnRtRhTypeChanged(EnumRtRhMode mode)
        {
            rtRhMode = mode;
            if (rtrhData == null)
            {
                Debug.LogWarning($"DataModelBinder_RTRH: pointModelData is null. Cannot update heat source.", this);
                return;
            }
            OnRtRhTypeChangedAction?.Invoke(rtRhMode);
            UpdateHeatSource();
        }

        #region Event Listeners
        private void OnEnable()
        {
            WebApiRealtimeDataHandler_RtRh.OnGetRealtimeAssetAction += OnGetRealtimeAssetAction;
            RtRhDataManager.OnRtRhModeChangedAction += OnRtRhTypeChanged;
        }

        private void OnDisable()
        {
            WebApiRealtimeDataHandler_RtRh.OnGetRealtimeAssetAction -= OnGetRealtimeAssetAction;
            RtRhDataManager.OnRtRhModeChangedAction -= OnRtRhTypeChanged;
        }
        #endregion
    }
}
