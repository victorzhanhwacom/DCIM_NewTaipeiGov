using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 機櫃與設備資產資料
    /// </summary>
    public class WebAPI_CallerBase_RackAndEquipment : WebAPI_CallerBase<WebAPI_Data_RackDTO>
    {
        public static Action<List<DCR_Asset>> OnGetDCRAssetDataAction;

        [SerializeField] private List<DCR_Asset> dcrAssets = new List<DCR_Asset>();

        public List<DCR_Asset> DCRAssets() => dcrAssets;

        override public void ParseJson(string json)
        {
            base.ParseJson(json);
            dcrAssets?.Clear();
            dcrAssets ??= new List<DCR_Asset>();
            webapiData.ForEach(dto =>
            {
                DCR_Asset dcrAsset = dto.ToDCRAsset();
                if (dcrAsset != null) dcrAssets.Add(dcrAsset);
            });
        }

        public override void InvokeData()
        {
            base.InvokeData();
            OnGetDCRAssetDataAction?.Invoke(dcrAssets);
        }
    }

    [Serializable]
    public class WebAPI_Data_RackDTO
    {
        [JsonProperty][field: SerializeField] public string deviceName { get; protected set; }
        [JsonProperty][field: SerializeField] public string deviceCode { get; protected set; }
        [JsonProperty][field: SerializeField] public string buildingCode { get; protected set; }
        [JsonProperty][field: SerializeField] public string floor { get; protected set; }
        [JsonProperty][field: SerializeField] public string space { get; protected set; }
        [JsonProperty][field: SerializeField] public int usedU { get; protected set; }
        [JsonProperty][field: SerializeField] public int totalU { get; protected set; }
        [JsonProperty][field: SerializeField] public float wattUsed { get; protected set; }
        [JsonProperty][field: SerializeField] public float wattLimit { get; protected set; }
        [JsonProperty][field: SerializeField] public float weightUsed { get; protected set; }
        [JsonProperty][field: SerializeField] public float weightLimit { get; protected set; }
        [JsonProperty][field: SerializeField] public List<MountedDevices> mountedDevices { get; protected set; }

        public DCR_Asset ToDCRAsset()
        {
            DCR_Asset result = new DCR_Asset
            {
                deviceCode = deviceCode,
                deviceName = deviceName,
                cobieInfo = new COBieInfo(),
                weight_kg_Max = weightLimit,
                power_watt_Max = wattLimit,
                u_height_Max = totalU,
                container = mountedDevices?.Select(dto => dto.ToEquipmentAsset()).ToList() ?? new List<EquipmentAsset>()
            };
            result.CheckSystemAndCategory();
            return result;
        }
    }

    [Serializable]
    public class MountedDevices
    {
        [JsonProperty][field: SerializeField] public string deviceName { get; protected set; }
        [JsonProperty][field: SerializeField] public string deviceCode { get; protected set; }
        [JsonProperty][field: SerializeField] public string systemType { get; protected set; }
        [JsonProperty][field: SerializeField] public string deviceCategory { get; protected set; }
        [JsonProperty][field: SerializeField] public string deviceModel { get; protected set; }
        [JsonProperty][field: SerializeField] public int rackLocationU { get; protected set; }
        [JsonProperty][field: SerializeField] public int heightU { get; protected set; }
        [JsonProperty][field: SerializeField] public float wattW { get; protected set; }
        [JsonProperty][field: SerializeField] public float wattLimit { get; protected set; }
        [JsonProperty][field: SerializeField] public float weightKg { get; protected set; }

        public EquipmentAsset ToEquipmentAsset()
        {
            EquipmentAsset result = new EquipmentAsset
            {
                deviceCode = deviceCode,
                deviceName = deviceName,
                system = systemType,
                category = deviceCategory,


                cobieInfo = new COBieInfo()
                {
                    type_modelNumber = deviceModel,
                    type_name = deviceName,
                },

                startUIndex = rackLocationU,
                equipmentUsageInfo = new EquipmentUsageInfo
                {
                    heightU = heightU,
                    weight_kg = weightKg,
                    power_watt = wattW
                },
                modelInfo = new ModelInfo
                {
                    modelName = DCIM_Helper.GetModelNameFromDeviceCode(deviceCode)
                },
            };
            result.CheckSystemAndCategory();
            return result;
        }
    }
}
