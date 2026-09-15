using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using VzDev.Frameworks;
using VzDev.StringUtils;
using VzDev.UnityAPI.Extensions;

namespace VzDev
{
    /// <summary>
    /// 全球人壽 庫存設備資料
    /// </summary>
    public class WebApiData_StockEquipmentHandler : SingletonMonoBehaviour<WebApiData_StockEquipmentHandler>
    {
        [SerializeField] private StockEquipmentDTO[] StockEquipmentData;
        [SerializeField] private EquipmentAsset[] equipmentAssets;

        public void ParseJson(string json)
        {
            StockEquipmentData = new StockEquipmentDTO[0];
            equipmentAssets = new EquipmentAsset[0];

            json = JsonHelper.GetJsonFromNode(json, "items");
            StockEquipmentData = JsonConvert.DeserializeObject<StockEquipmentDTO[]>(json);
            equipmentAssets = StockEquipmentData.Select(data => data.ToEquipmentAsset()).ToArray();
            OnGetStockeEquipmentListAction?.Invoke(equipmentAssets);
        }

        /// <summary>
        /// 庫存設備資產列表
        /// </summary>
        public static EquipmentAsset[] GetStockEquipmentAssets() => Instance.equipmentAssets;

        public static Action<EquipmentAsset[]> OnGetStockeEquipmentListAction;
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
