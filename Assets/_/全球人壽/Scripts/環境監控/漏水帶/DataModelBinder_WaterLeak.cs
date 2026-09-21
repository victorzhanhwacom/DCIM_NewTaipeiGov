using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VzDev.DCIMUtils.Extensions;
using static VzDev.DCIMUtils.EnviornmentUtils.RtRhDataManager;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 漏水帶數據綁定器：模型
    /// </summary>
    public class DataModelBinder_WaterLeak : MonoBehaviour
    {
        #region Events
        public Action<RealtimeAsset_WaterLeak> OnWaterLeakDataChangedAction;
        #endregion

        #region Fields
        [SerializeField, ReadOnly] private EnumRtRhMode rtRhMode = EnumRtRhMode.Unselect;
        [SerializeField, ReadOnly] private RealtimeAsset_WaterLeak waterLeakData;

        public RealtimeAsset_WaterLeak WaterLeakData => waterLeakData;
        #endregion
        
        /// <summary>
        /// WebAPI回傳即時資料時
        /// </summary>
        private void OnGetRealtimeAssetAction(RealtimeAsset_WaterLeak[] list)
        {
            string deviceCode = transform.parent.GetModelDeviceCode();
            waterLeakData = list.FirstOrDefault(data => data.deviceCode == deviceCode);
            if (waterLeakData == null)
            {
                Debug.LogWarning($"DataModelBinder_RTRH: No matching data found for deviceCode in the list.", this);
                return;
            }
            waterLeakData.modelInfo = new DataUtils.ModelInfo
            {
                modelTarget = transform.parent,
                modelName = transform.parent.name,
            };
            OnWaterLeakDataChangedAction?.Invoke(waterLeakData);
        }

     
        #region Event Listeners
        private void OnEnable()
        {
            WebApiRealtimeDataHandler_WLK.OnGetRealtimeAssetAction += OnGetRealtimeAssetAction;
        }

        private void OnDisable()
        {
            WebApiRealtimeDataHandler_WLK.OnGetRealtimeAssetAction -= OnGetRealtimeAssetAction;
        }
        #endregion
    }
}
