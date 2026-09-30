using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using VzDev.Frameworks.ScrollRectUtils;
using VzDev.MathUtils;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI即時資料格式
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData: IDataKeyID
    {
        public WebAPI_RealtimeData_UPSHost ToUPSHost() => CloneAs<WebAPI_RealtimeData_UPSHost>();
        public WebAPI_RealtimeData_UPSBattery ToUPSBattery() => CloneAs<WebAPI_RealtimeData_UPSBattery>();
        public WebAPI_RealtimeData_RtRh ToRtRh() => CloneAs<WebAPI_RealtimeData_RtRh>();
        public WebAPI_RealtimeData_CRAC ToCRAC() => CloneAs<WebAPI_RealtimeData_CRAC>();
        public WebAPI_RealtimeData_InRowCooler ToInRowCooler() => CloneAs<WebAPI_RealtimeData_InRowCooler>();

        public string dataKeyID => deviceCode;

        /// <summary>
        /// 0: 正常, 1: 告警, 2: 離線
        /// </summary>
        public virtual int TotalAlertLevelStatus { get;}

        /// <summary>
        /// 計算多個Tag的總告警等級
        /// <para>+ 0: 正常, 1: 告警, 2: 離線</para>
        /// </summary>
        protected int GetTotalAlertLevelStatus(params Tags[] tags)
        {
            if (tags == null || tags.Length == 0)
                return 0;

            List<int> alertLevels = tags.Select(tag => tag.alertLevelStatus).ToList();
            return MathHelper.Max(alertLevels);
        }

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

        #region Fields
        [JsonProperty]
        [field: SerializeField]
        public string deviceName { get; protected set; }
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