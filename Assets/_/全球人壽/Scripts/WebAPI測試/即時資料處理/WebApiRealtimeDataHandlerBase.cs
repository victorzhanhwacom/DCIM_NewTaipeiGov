using System;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using VzDev.Frameworks;
using VzDev.NetUtils;
using VzDev.StringUtils;

namespace VzDev
{
    /// <summary>
    /// WebAPI 即時資料處理基底類別
    /// </summary>
    public abstract class WebApiRealtimeDataHandlerBase<TAsset> : SingletonMonoBehaviour<WebApiRealtimeDataHandlerBase<TAsset>>
        where TAsset : RealtimeAsset, new()
    {
        #region Field
        [Foldout("[Events]"), SerializeField] protected UnityEvent<bool> onCallingEvent;
        [Foldout("[Events]"), SerializeField] protected UnityEvent<string> onErrorEvent;

        [SerializeField, Expandable] protected WebApiRequestSO webApiRequestSO;
        [Foldout("[Response]"), SerializeField] protected RealTimeDataDTO[] rawData;
        [Foldout("[Response]"), SerializeField] protected TAsset[] assets;
        protected bool isHaveRequest => webApiRequestSO != null;
        protected bool isApiCalling;
        #endregion


        [Button, ShowIf("isHaveRequest"), DisableIf("isApiCalling")]
        public void CallWebAPI()
        {
            isApiCalling = true;
            onCallingEvent?.Invoke(isApiCalling);
            webApiRequestSO.CallAPI(ParseJson, OnFailed);
        }

        public void ParseJson(string json)
        {
            rawData = new RealTimeDataDTO[0];
            assets = new TAsset[0];
            json = JsonHelper.GetJsonFromNode(json, "devices");
            rawData = JsonConvert.DeserializeObject<RealTimeDataDTO[]>(json);
            assets = rawData.Select(data => data.ToAsset<TAsset>()).ToArray();

            isApiCalling = false;
            onCallingEvent?.Invoke(isApiCalling);
            OnGetRealtimeAssetAction?.Invoke(assets);
        }

        private void OnFailed(string message)
        {
            isApiCalling = false;
            onCallingEvent?.Invoke(isApiCalling);
            onErrorEvent?.Invoke(message);
        }

        #region Static
        public static TAsset[] RealtimeAsset() => Instance.assets;
        public static Action<TAsset[]> OnGetRealtimeAssetAction;
        #endregion
    }
}
