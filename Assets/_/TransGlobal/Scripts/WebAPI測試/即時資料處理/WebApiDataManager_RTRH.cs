using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using VzDev.EventUtils;
using VzDev.Frameworks;
using VzDev.InterfaceUtils;
using VzDev.NetUtils;
using VzDev.StringUtils;

namespace VzDev
{
    /// <summary>
    /// WebAPI原始資料基底類別
    /// <para> + 轉換成資料對像 </para>
    /// </summary>
    public class WebApiDataManager_RTRH : WebApiDataManagerBase
            <WebAPI_RawData_Realtime<WebApiData_RTRH>, WebApiData_RTRH>
    {
    }

    [Serializable]
    public class WebAPI_RawData_Realtime<T> : WebAPI_RawDataBase<T>
    {
        public override T Convert() => default;

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
            private void OnDeserialized(StreamingContext context)
            => localTimestamp = localTimestamp.Replace("T", " ");

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


    [Serializable]
    public class WebAPI_RawData_RTRH : WebAPI_RawDataBase<WebApiData_RTRH>
    {
        public override WebApiData_RTRH Convert()
        {
            return new WebApiData_RTRH()
            {

            };
        }
    }

    [Serializable]
    public class WebApiData_RTRH : DCIMAsset
    {
    }
}
