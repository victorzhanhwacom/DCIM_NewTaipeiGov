using System;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VictorDev.Net.WebAPI;
using VzDev.Frameworks;

namespace VzDev.TGL
{
    /// <summary>
    /// 全球人壽 登入用戶資料
    /// </summary>
    public class TGL_LoginUser : SingletonMonoBehaviour<TGL_LoginUser>
    {
        [SerializeField, ReadOnly] private TGL_LoginUserData loginUserData;
        [SerializeField, Expandable] private WebApiAuthorizationSO webApiAuthorization;

        public void ParseJson(string json)
        {
            loginUserData = default;
            loginUserData = JsonConvert.DeserializeObject<TGL_LoginUserData>(json);
            webApiAuthorization?.SetToken("");
            webApiAuthorization?.SetToken(loginUserData.accessToken);
        }
    }

    [Serializable]
    public struct TGL_LoginUserData
    {
        [JsonProperty]
        [field: SerializeField]
        public string accessToken { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string accessTokenExpiresAt { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string refreshToken { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string refreshTokenExpiresAt { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public User user { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string step { get; private set; }
    }

    [Serializable]
    public struct User
    {
        [JsonProperty]
        [field: SerializeField]
        public string userId { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string account { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string name { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public bool isFirstLogin { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string[] roles { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string[] permissions { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public DeviceSystems[] deviceSystems { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public bool mustChangePassword { get; private set; }
    }

    [Serializable]
    public struct DeviceSystems
    {
        [JsonProperty]
        [field: SerializeField]
        public string code { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public string name { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public bool canView { get; private set; }
        [JsonProperty]
        [field: SerializeField]
        public bool canControl { get; private set; }
    }
}
