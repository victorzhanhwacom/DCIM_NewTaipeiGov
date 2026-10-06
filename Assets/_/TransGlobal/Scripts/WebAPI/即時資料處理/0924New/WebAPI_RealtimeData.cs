using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using VzDev.Frameworks.ScrollRectUtils;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI即時資料格式
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData : IDataKeyID
    {
        public WebAPI_RealtimeData_UPSHost ToUPSHost() => CloneAs<WebAPI_RealtimeData_UPSHost>();
        public WebAPI_RealtimeData_UPSBattery ToUPSBattery() => CloneAs<WebAPI_RealtimeData_UPSBattery>();
        public WebAPI_RealtimeData_RtRh ToRtRh() => CloneAs<WebAPI_RealtimeData_RtRh>();
        public WebAPI_RealtimeData_WaterLeak ToWaterLeak() => CloneAs<WebAPI_RealtimeData_WaterLeak>();
        public WebAPI_RealtimeData_FS ToFS() => CloneAs<WebAPI_RealtimeData_FS>();
        public WebAPI_RealtimeData_GasCylinder ToGasCylinder() => CloneAs<WebAPI_RealtimeData_GasCylinder>();
        public WebAPI_RealtimeData_CCTV ToCCTV() => CloneAs<WebAPI_RealtimeData_CCTV>();
        public WebAPI_RealtimeData_Door ToDoor() => CloneAs<WebAPI_RealtimeData_Door>();
        public WebAPI_RealtimeData_RackDoor ToRackDoor() => CloneAs<WebAPI_RealtimeData_RackDoor>();

        public string dataKeyID => deviceCode;

        /// <summary>
        /// 所有Tag的alertLevel總告警等級
        /// <para>+ 0: 正常, 1: 告警, 2: 離線</para>
        /// </summary>
        public int TotalAlertLevelStatus => GetTotalAlertLevelStatus(tags);

        protected int GetRandomTotalAlertLevelStatus()
        {
            float result = UnityEngine.Random.Range(0f, 1f); 
            if (result < 0.9f) return 0;
            if (result < 0.95f) return 1;
            return 2;
        }

        /// <summary>
        /// 計算多個Tag的總告警等級
        /// <para>+ 0: 正常, 1: 告警, 2: 離線</para>
        /// </summary>
        protected int GetTotalAlertLevelStatus(params Tag[] tags) => GetTotalAlertLevelStatus((IReadOnlyList<Tag>)tags);
        // 給已經有 List 的呼叫端，避免 ToArray() 產生額外配置
        protected int GetTotalAlertLevelStatus(IReadOnlyList<Tag> tags)
        {
            if (tags == null || tags.Count == 0) return 0;
            int maxAlertLevelStatus = 0;

            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].alertLevelStatus > maxAlertLevelStatus)
                {
                    maxAlertLevelStatus = tags[i].alertLevelStatus;
                }
            }
            return maxAlertLevelStatus;
        }

        /// <summary>
        /// 轉型並複製資料
        /// </summary>
        public T CloneAs<T>() where T : WebAPI_RealtimeData, new()
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

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
#if UNITY_EDITOR
            // 將tag依照tagId排序，方便後續使用
            tags?.Sort((a, b) => a.tagId.CompareTo(b.tagId));
#endif
        }

        #region Fields
        [JsonProperty]
        [field: SerializeField]
        public string deviceName { get; protected set; }
        public void SetDeviceName(string newDeviceName) => deviceName = newDeviceName;
        public ModelInfo modelInfo;
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
        public List<Tag> tags { get; protected set; }
        #endregion

        [Serializable]
        public class Tag
        {
            [OnDeserialized]
            protected void OnDeserialized(StreamingContext context)
            {
                value = value?.Trim();
                if (!string.IsNullOrEmpty(localTimestamp))
                {
                    localTimestamp = localTimestamp.Replace("T", " ");
                }

                if (string.IsNullOrEmpty(value))
                {
                    value = "---";
                }

                // 如果value為float型態時，將其四捨五入到小數點後一位
                if (float.TryParse(value, out float floatValue))
                {
                    value = floatValue.RoundToDecimals(1).ToString();
                }
            }

            /// <summary>
            /// 告警狀態： 0: 正常, 1: 告警, 2: 離線
            /// </summary>
            public int alertLevelStatus
            {
                get
                {
                    switch (alertLevel)
                    {
                        case 0: return 0; // 正常
                        case 99: return 2; // 離線
                        default: return 1; // 告警
                    }
                }
            }

            public void SetValue(string newValue) => value = newValue;

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
            public string valueWithUnit => string.IsNullOrEmpty(unit) ? value : $"{value} {unit}";  
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
            /// <summary>
            /// 告警狀態： 0: 正常, 1~98: 告警, 99: 離線
            /// </summary>
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