using System;
using System.Linq;
using System.Runtime.Serialization;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DateTimeUtils;
using VzDev.DCIMUtils.DataUtils;
using VzDev.Frameworks;
using VzDev.StringUtils;

namespace VzDev.TGL
{
    /// <summary>
    /// 全球人壽 庫存設備資料
    /// </summary>
    public class TGL_StockEquipmentHandler : SingletonMonoBehaviour<TGL_StockEquipmentHandler>
    {
        [SerializeField] private TGL_StockEquipmentDTO[] tgl_StockEquipmentData;
        [SerializeField] private EquipmentAsset[] equipmentAssets;

        public void ParseJson(string json)
        {
            tgl_StockEquipmentData = new TGL_StockEquipmentDTO[0];
            equipmentAssets = new EquipmentAsset[0];

            tgl_StockEquipmentData = JsonConvert.DeserializeObject<TGL_StockEquipmentDTO[]>(json);
            equipmentAssets = tgl_StockEquipmentData.Select(data => data.ToEquipmentAsset()).ToArray();
            OnGetStockeEquipmentListAction?.Invoke(equipmentAssets);
        }

        /// <summary>
        /// 庫存設備資產列表
        /// </summary>
        public static EquipmentAsset[] GetStockEquipmentAssets() => Instance.equipmentAssets;

        public static Action<EquipmentAsset[]> OnGetStockeEquipmentListAction;
    }

    [Serializable]
    public struct TGL_StockEquipmentDTO
    {
        public EquipmentAsset ToEquipmentAsset()
        {
            EquipmentAsset asset = new EquipmentAsset()
            {
                deviceName = modelName,
                system = Enum.TryParse<DCIM_System>(system, out var parsedSystem) ? parsedSystem : DCIM_System.Unknow,
                equipmentUsageInfo = new EquipmentUsageInfo()
                {
                    heightU = heightU,
                    power_watt = power,
                    weight_kg = weight
                },
                modelInfo = new ModelInfo()
                {
                    modelName = modelName,
                },
                cobieInfo = new COBieInfo()
                {
                    type_manufacturer = brand,
                },
                deploymentStatus = DeploymentStatus.InStock,
            };
            return asset;
        }

        [JsonProperty]
        [field: SerializeField]
        public string modelName { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string brand { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string system { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public float power { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public float weight { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public int heightU { get; private set; }
    }
}
