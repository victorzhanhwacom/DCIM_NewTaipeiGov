using System;
using System.Collections.Generic;
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

        [Foldout("[Settings]"), SerializeField] private Material[] levelMaterials;
        [SerializeField] private LineRenderer lineRenderer;

        public RealtimeAsset_WaterLeak WaterLeakData => waterLeakData;
        #endregion

        private void Start() => lineRenderer = transform.parent.GetComponent<LineRenderer>();

        /// <summary>
        /// WebAPI回傳即時資料時
        /// </summary>
        private void OnGetRealtimeAssetAction(List<RealtimeAsset_WaterLeak> list)
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

            lineRenderer = transform.parent.GetComponent<LineRenderer>();
            lineRenderer.material = levelMaterials[waterLeakData.alertStatus == 0 ? 0 : 1];
        }


        #region Event Listeners
        private void OnEnable()
        {
            WebApiRealtimeDataHandler_WLK.OnGetWebApiDataAction += OnGetRealtimeAssetAction;
        }

        private void OnDisable()
        {
            WebApiRealtimeDataHandler_WLK.OnGetWebApiDataAction -= OnGetRealtimeAssetAction;
        }
        #endregion
    }
}
