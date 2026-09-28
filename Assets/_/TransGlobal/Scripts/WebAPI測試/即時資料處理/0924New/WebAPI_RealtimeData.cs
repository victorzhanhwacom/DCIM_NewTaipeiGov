using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI即時資料格式
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData
    {
        public WebAPI_RealtimeData_UPSHost ToUPSHost() => CloneAs<WebAPI_RealtimeData_UPSHost>();
        public WebAPI_RealtimeData_UPSBattery ToUPSBattery() => CloneAs<WebAPI_RealtimeData_UPSBattery>();
        public WebAPI_RealtimeData_RtRh ToRtRh() => CloneAs<WebAPI_RealtimeData_RtRh>();
        public WebAPI_RealtimeData_CRAC ToCRAC() => CloneAs<WebAPI_RealtimeData_CRAC>();
        public WebAPI_RealtimeData_InRowCooler ToInRowCooler() => CloneAs<WebAPI_RealtimeData_InRowCooler>();


        /// <summary>
        /// 轉型並複製資料
        /// </summary>
        protected T CloneAs<T>() where T : WebAPI_RealtimeData, new()
        {
            return new T()
            {
                deviceName = deviceName,
                deviceId = deviceId,
                deviceCode = deviceCode,
                systemType = systemType,
                deviceCategory = deviceCategory,
                deviceModel = deviceModel,
                tags = tags,
                modelInfo = modelInfo
            };
        }


        public enum EnumRealtimeAlertLevel
        {
            Normal = 0,
            Alert,
            Disconnect = 99
        }

        public ModelInfo modelInfo;

        #region Fields
        [JsonProperty]
        [field: SerializeField]
        public string deviceName { get; protected set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceId { get; protected set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceCode { get; protected set; }
        [JsonProperty]
        [field: SerializeField]
        public string systemType { get; protected set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceCategory { get; protected set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceModel { get; protected set; }
        [JsonProperty]
        [field: SerializeField]
        public Tags[] tags { get; protected set; }
        #endregion

        [Serializable]
        public class Tags
        {
            [OnDeserialized]
            protected void OnDeserialized(StreamingContext context)
            {
                if (!string.IsNullOrEmpty(localTimestamp))
                {
                    localTimestamp = localTimestamp.Replace("T", " ");
                }

                if (string.IsNullOrEmpty(value))
                {
                    value = "---";
                }
            }

            /// <summary>
            /// 告警狀態： 0: 正常, 1~98: 告警, 99: 離線
            /// </summary>
            public EnumRealtimeAlertLevel alertLevelStatus => (EnumRealtimeAlertLevel)alertLevel;

            #region Fields
            [JsonProperty]
            [field: SerializeField]
            public string displayName { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string tagId { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string name { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string value { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string valueKind { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string unit { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string timestamp { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string localTimestamp { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string sourceType { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public bool isOfflineStatusTag { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public int alertLevel { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string severity { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string message { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string triggeredAt { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public int direction { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string valueLabels { get; private set; }
            #endregion
        }
    }
}