using System;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VictorDev.Net;
using VictorDev.Net.WebAPI;
using VzDev.Frameworks;

namespace VzDev.TGL
{
    /// <summary>
    /// 全球人壽 Config設定
    /// </summary>
    public class TGL_Config : SingletonMonoBehaviour<TGL_Config>
    {
        [SerializeField, ReadOnly] private TGL_ConfigData config;
        [SerializeField, Expandable] private IPConfigSO ipConfig;

        public void ParseJson(string json)
        {
            config = default;
            config = JsonConvert.DeserializeObject<TGL_ConfigData>(json);

            ipConfig?.SetConfig("", "", "");
            ipConfig?.SetConfig(config.httpType, config.ip, config.port);
        }

        public static bool IsDemo => Instance.config.isDemo == "1";
    }

    [Serializable]
    public struct TGL_ConfigData
    {
        #region JsonProperty
        [JsonProperty]
        [field: SerializeField]
        public string isDemo { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string httpType { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string ip { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string port { get; private set; }
        #endregion
    }
}
