using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 庫存設備資產資料
    /// </summary>
    public class WebAPI_CallerBase_StockEquipment : WebAPI_CallerBase<WebAPI_Data_StockEquipmentDTO>
    {
        public static Action<List<EquipmentAsset>> OnGetDCRAssetDataAction;

        [SerializeField] private List<EquipmentAsset> stockEquipmentList = new List<EquipmentAsset>();

        public List<EquipmentAsset> StockEquipmentList() => stockEquipmentList;

        override public void ParseJson(string json)
        {
            base.ParseJson(json);
            stockEquipmentList?.Clear();
            stockEquipmentList ??= new List<EquipmentAsset>();
            webapiData.ForEach(dto =>
            {
                EquipmentAsset dcrAsset = dto.ToEquipmentAsset();
                if (dcrAsset != null) stockEquipmentList.Add(dcrAsset);
            });
        }

        public override void InvokeData()
        {
            base.InvokeData();
            OnGetDCRAssetDataAction?.Invoke(stockEquipmentList);
        }
    }

    [Serializable]
    public class WebAPI_Data_StockEquipmentDTO
    {
        [JsonProperty][field: SerializeField] public string deviceName { get; protected set; }
        [JsonProperty][field: SerializeField] public string deviceCode { get; protected set; }
        [JsonProperty][field: SerializeField] public string buildingCode { get; protected set; }
        [JsonProperty][field: SerializeField] public string floor { get; protected set; }
        [JsonProperty][field: SerializeField] public string space { get; protected set; }
        [JsonProperty][field: SerializeField] public string systemType { get; protected set; }
        [JsonProperty][field: SerializeField] public string deviceCategory { get; protected set; }
        [JsonProperty][field: SerializeField] public string deviceModel { get; protected set; }
        [JsonProperty][field: SerializeField] public int heightU { get; protected set; }
        [JsonProperty][field: SerializeField] public float wattW { get; protected set; }
        [JsonProperty][field: SerializeField] public float weightKg { get; protected set; }
        [JsonProperty][field: SerializeField] public string assetNumber { get; protected set; }
        [JsonProperty][field: SerializeField] public string information { get; protected set; }

        public EquipmentAsset ToEquipmentAsset()
        {
            EquipmentAsset result = new EquipmentAsset
            {
                deviceCode = deviceCode,
                deviceName = deviceName,
                system = systemType,
                category = deviceCategory,

                equipmentUsageInfo = new EquipmentUsageInfo
                {
                    power_watt = wattW,
                    weight_kg = weightKg,
                    heightU = heightU
                },
            };
            result.CheckSystemAndCategory();
            return result;
        }
    }
}


