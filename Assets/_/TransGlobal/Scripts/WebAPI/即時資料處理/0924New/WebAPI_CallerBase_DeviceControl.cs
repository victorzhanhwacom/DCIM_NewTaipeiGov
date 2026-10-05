using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 控制設備項(依照tagId : onf)
    /// </summary>
    public class WebAPI_CallerBase_DeviceControl : WebAPI_CallerBase<DeviceControlResult>
    {
        private static Action<List<DeviceControlResult>> onSuccessAction;

        #region 設定設備控制
        /// <summary>
        /// 設定設備控制
        /// </summary>
        public static void SetDeviceControl(DeviceControl deviceControls, Action<List<DeviceControlResult>> onSuccess, Action<string> onError)
            => SetDeviceControl(new List<DeviceControl> { deviceControls }, onSuccess, onError);
        /// <summary>
        /// 設定設備控制
        /// </summary>
        public static void SetDeviceControl(string tagId, bool isOn, Action<List<DeviceControlResult>> onSuccess, Action<string> onError)
            => SetDeviceControl(new List<DeviceControl> { new DeviceControl(tagId, isOn) }, onSuccess, onError);
        /// <summary>
        /// 設定設備控制
        /// </summary>
        public static void SetDeviceControl(List<DeviceControl> deviceControls, Action<List<DeviceControlResult>> onSuccess, Action<string> onError)
        {
            onSuccessAction = onSuccess;
            var itemsWrapper = new ItemsWrapper { items = deviceControls };
            string jsonData = JsonConvert.SerializeObject(itemsWrapper);
            Debug.Log($"[WebAPI_CallerBase_DeviceControl] SetDeviceControl: {jsonData}");

            Instance.SetBodyRawJson(jsonData);
            Instance.CallWebAPI(OnCallApiSuccess, onError);
        }
        #endregion

        private static void OnCallApiSuccess(string response)
        {
            JObject jsonResponse = JObject.Parse(response);
            string resultJson = jsonResponse["results"]?.ToString();
            List<DeviceControlResult> result = JsonConvert.DeserializeObject<List<DeviceControlResult>>(resultJson);
            if (result != null && result.Count > 0)
            {
                onSuccessAction?.Invoke(result);
            }
            else
            {
                Debug.LogWarning($"[WebAPI_CallerBase_DeviceControl] OnCallApiSuccess: response is null or empty. Response: {response}");
            }
        }
    }

    [Serializable]
    public class ItemsWrapper
    {
        public List<DeviceControl> items;
    }

    [Serializable]
    public class DeviceControl
    {
        public string tagId, value;
        public DeviceControl(string tagId, bool isOn)
        {
            this.tagId = tagId;
            value = isOn ? "1" : "0";
        }

        public DeviceControl(string tagId, float value)
        {
            this.tagId = tagId;
            this.value = value.ToString();
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

        /// <summary>
        /// 是否修改成功
        /// </summary>
        public bool IsSuccess => status == 0;
    }
}


