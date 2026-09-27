using System;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 控制設備項(依照tagId : onf)
    /// </summary>
    public class WebAPI_CallerBase_DeviceControl : WebAPI_CallerBase<DeviceControlResult>
    {
    }

    [Serializable]
    public class ItemsWrapper
    {
        public DeviceControl[] items;
    }

    [Serializable]
    public class DeviceControl
    {
        public string tagId;
        public string value;
        public DeviceControl(string tagId, bool isOn)
        {
            this.tagId = tagId;
            value = isOn ? "1" : "0";
        }
    }

    /// <summary>
    /// 單筆控制結果
    /// Status 語意對應 TagWriteCommand.Status： 
    /// 0=Queued（非 Entek 點位，已寫入 TagWriteCommands 待派發佇列，非同步，實際派發結果請改用 GET api/ict/tag-assignments/{id}/commands 查詢）； 
    /// 1=Dispatched（Entek 點位，已同步呼叫 Entek 控制 API 且成功，終態）； 
    /// 2=Failed（驗證失敗，或 Entek 呼叫失敗，終態）。
    /// </summary>
    [Serializable]
    public class DeviceControlResult
    {
        [JsonProperty][field: SerializeField] public string tagId { get; protected set; }
        [JsonProperty][field: SerializeField] public int status { get; protected set; }
        [JsonProperty][field: SerializeField] public string error { get; protected set; }
    }
}


