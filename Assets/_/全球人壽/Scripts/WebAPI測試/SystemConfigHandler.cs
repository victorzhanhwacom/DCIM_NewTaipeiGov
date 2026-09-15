using System;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.Frameworks;
using VzDev.NetUtils;
using VzDev.StringUtils;

namespace VzDev
{
    /// <summary>
    /// 系統Config設定
    /// </summary>
    public class SystemConfigHandler : SingletonMonoBehaviour<SystemConfigHandler>
    {
        [SerializeField, ReadOnly] private SystemConfig config;
        [SerializeField, Expandable] private IPConfigSO ipConfig;

        public void ParseJson(string json)
        {
            config = default;

            json = JsonHelper.GetJsonFromNode(json, "system");
            config = JsonConvert.DeserializeObject<SystemConfig>(json);
            ipConfig?.SetConfig(config.httpType, config.ip, config.port);
        }
     //   public static bool IsDemo = Instance.config.isDemo;
    }

    [Serializable]
    public struct SystemConfig
    {
        #region JsonProperty
        [JsonProperty]
        [field: SerializeField]
        public bool isDemo { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string httpType { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string ip { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string port { get; private set; }
         [JsonProperty]
        [field: SerializeField]
        public string Surfix { get; private set; }
        #endregion
    }
}
