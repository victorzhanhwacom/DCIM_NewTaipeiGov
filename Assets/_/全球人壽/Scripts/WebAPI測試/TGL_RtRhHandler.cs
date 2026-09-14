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
    /// 全球人壽 登入用戶資料
    /// </summary>
    public class TGL_RtRhHandler : SingletonMonoBehaviour<TGL_RtRhHandler>
    {
        [SerializeField] private TGL_RtRhDataDTO[] tgl_RtRhData;
        [SerializeField] private RtRhAsset[] rtRhAssets;

        public void ParseJson(string json)
        {
            tgl_RtRhData = new TGL_RtRhDataDTO[0];
            rtRhAssets = new RtRhAsset[0];

            json = JsonHelper.GetJsonFromNode(json, "devices");
            tgl_RtRhData = JsonConvert.DeserializeObject<TGL_RtRhDataDTO[]>(json);
            rtRhAssets = tgl_RtRhData.Select(data => data.ToRtRhAsset()).ToArray();
        }
    }

    [Serializable]
    public struct TGL_RtRhDataDTO
    {
        public RtRhAsset ToRtRhAsset()
        {
            RtRhAsset.Tags rtTag = tags.FirstOrDefault(tag => tag.name == "dbt").ToRtRhTag();
            RtRhAsset.Tags rhTag = tags.FirstOrDefault(tag => tag.name == "rh").ToRtRhTag();

            RtRhAsset asset = new RtRhAsset()
            {
                deviceCode = deviceCode,
                deviceName = deviceName,
                system = Enum.TryParse<DCIM_System>(systemType, out var parsedSystem) ? parsedSystem : DCIM_System.Unknow,
                category = Enum.TryParse<DCIM_Catetory>(deviceCategory.ToUpper(), out var parsedCategory) ? parsedCategory : DCIM_Catetory.Unknow,
                rtTag = rtTag,
                rhTag = rhTag
            };
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

        [Serializable]
        public struct Tags
        {
            public RtRhAsset.Tags ToRtRhTag()
            {
                RtRhAsset.Tags result = new RtRhAsset.Tags()
                {
                    title = displayName,
                    value = value,
                    localTimestamp = localTimestamp.Replace("T", " "),
                    alert = new RtRhAsset.Alert()
                    {
                        alertLevel = alertLevel,
                        message = message,
                        severity = severity
                    }
                };
                return result;
            }

            [JsonProperty]
            [field: SerializeField]
            public string tagId { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public string name { get; private set; }
            [JsonProperty]
            [field: SerializeField]
            public float value { get; private set; }
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
        }

    }
}
