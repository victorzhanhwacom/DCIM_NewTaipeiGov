using System;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using VzDev.EventUtils;
using VzDev.Frameworks;
using VzDev.NetUtils;
using VzDev.StringUtils;

namespace VzDev
{
    /// <summary>
    /// 全球人壽 庫存設備資料
    /// </summary>
    public class WebApiData_StockEquipmentHandler : SingletonMonoBehaviour<WebApiData_StockEquipmentHandler>
    {
        #region Field
        [Label("[Events]"), SerializeField] private OnCallbackEvent onCallingEvent;
        [SerializeField, Expandable] protected WebApiRequestSO webApiRequestSO;
        [Foldout("[Response]"), SerializeField] private StockEquipmentDTO[] StockEquipmentData;
        [Foldout("[Response]"), SerializeField] private EquipmentAsset[] equipmentAssets;
        #endregion

        #region 呼叫行為事件
        protected bool isHaveRequest => webApiRequestSO != null;
        protected bool isApiCalling;
        [Button, ShowIf("isApiCalling")]
        private void CancelCalling() => isApiCalling = false;
        [Button, ShowIf("isHaveRequest"), DisableIf("isApiCalling")]
        public void CallWebAPI()
        {
            isApiCalling = true;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            webApiRequestSO.CallAPI(ParseJson, OnFailed);
        }
        private void OnFailed(string message)
        {
            isApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            onCallingEvent.InvokeOnErrorEvent(message);
        }
        #endregion

        public void ParseJson(string json)
        {
            StockEquipmentData = new StockEquipmentDTO[0];
            equipmentAssets = new EquipmentAsset[0];

            json = JsonHelper.GetJsonFromNode(json, "items");
            StockEquipmentData = JsonConvert.DeserializeObject<StockEquipmentDTO[]>(json);
            equipmentAssets = StockEquipmentData.Select(data => data.ToEquipmentAsset()).ToArray();
            OnGetStockeEquipmentListAction?.Invoke(equipmentAssets);
        }

        #region Static 資料存取 / 事件
        /// <summary>
        /// 庫存設備資產列表
        /// </summary>
        public static EquipmentAsset[] GetStockEquipmentAssets() => Instance.equipmentAssets;

        public static Action<EquipmentAsset[]> OnGetStockeEquipmentListAction;
        #endregion
    }

    /// <summary>
    /// WebAPI 庫存設備資料 DTO
    /// </summary>
    [Serializable]
    public struct StockEquipmentDTO
    {
        public EquipmentAsset ToEquipmentAsset()
        {
            EquipmentAsset asset = new EquipmentAsset()
            {
                deviceCode = deviceCode,
                deviceName = deviceName,
                system = systemType,
                category = deviceCategory,

                equipmentUsageInfo = new EquipmentUsageInfo()
                {
                    heightU = heightU,
                    power_watt = wattW,
                    weight_kg = weightKg
                },
                modelInfo = new ModelInfo()
                {
                    modelName = deviceModel,
                },
                companyAssetInfo = new CompanyAssetInfo()
                {
                    assetName = deviceName,
                    assetNumber = assetNumber
                },
            };

            asset.cobieInfo ??= new COBieInfo();
            asset.cobieInfo.CheckAndFixData(asset);
            if (string.IsNullOrEmpty(asset.companyAssetInfo.assetNumber))
                asset.companyAssetInfo.GenerateRandomAssetNumber("NTCGO");

            return asset;
        }

        [JsonProperty]
        [field: SerializeField]
        public string deviceCode { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceName { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string buildingCode { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string floor { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string space { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public DCIM_System systemType { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public DCIM_Category deviceCategory { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceModel { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public int heightU { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public float wattW { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public float weightKg { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string assetNumber { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public COBieInfo information { get; private set; }
    }
}
