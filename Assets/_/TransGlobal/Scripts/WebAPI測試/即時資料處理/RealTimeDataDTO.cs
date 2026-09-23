using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;

namespace VzDev
{
    [Serializable]
    public class RealTimeDataDTO
    {
        /// <summary>
        /// 將 WebAPI即時資料轉換成指定的RealtimeAsset
        /// </summary>
        public T ToAsset<T>() where T : RealtimeAsset, new()
        {
            T result = new T()
            {
                deviceCode = deviceCode,
                deviceName = deviceName,
                system = Enum.TryParse<DCIM_System>(systemType, out var parsedSystem) ? parsedSystem : DCIM_System.Unknow,
                category = Enum.TryParse<DCIM_Category>(deviceCategory.ToUpper(), out var parsedCategory) ? parsedCategory : DCIM_Category.Unknow,
            };
            result.SetTags(tags);
            return result;
        }

        #region Fields
        [JsonProperty]
        [field: SerializeField]
        public string deviceCode { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceName { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string systemType { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceCategory { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string deviceModel { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public Tags[] tags { get; private set; }
        #endregion

        [Serializable]
        public struct Tags
        {
            [OnDeserialized]
            private void OnDeserialized(StreamingContext context) => localTimestamp = localTimestamp.Replace("T", " ");

            #region Fields
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
            public string displayName { get; private set; }
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