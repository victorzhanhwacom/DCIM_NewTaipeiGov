using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.EventUtils;
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
        [Label("[Events]"), SerializeField] protected OnCallbackEvent onCallingEvent = new OnCallbackEvent();

        [SerializeField, Expandable] protected WebApiRequestSO webApiRequestSO;
        [Foldout("[Response]"), SerializeField] protected RealTimeDataDTO[] rawData;
        [Foldout("[Response]"), SerializeField] protected List<TAsset> assets;

        public static List<TAsset> RealtimeAssets => Instance.assets;

        #endregion

        #region 呼叫行為事件
        protected bool isHaveRequest => webApiRequestSO != null;
        protected bool isApiCalling;
        [Button, ShowIf("isApiCalling")]
        private void CancelCalling() => isApiCalling = false;

        public void StopCallApi()
        {
            isApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            webApiRequestSO.StopCallApi();
        }

        [Button, ShowIf("isHaveRequest"), DisableIf("isApiCalling")]
        public void CallWebAPI()
        {
            isApiCalling = true;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            webApiRequestSO.CallAPI(ParseJson, OnFailed);
        }
        private void OnFailed(string message)
        {
            isApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            onCallingEvent.InvokeOnErrorEvent(message);
        }
        #endregion

        public void ParseJson(string json)
        {
            rawData = new RealTimeDataDTO[0];
            assets = new List<TAsset>();
            json = JsonHelper.GetJsonFromNode(json, "devices");
            rawData = JsonConvert.DeserializeObject<RealTimeDataDTO[]>(json);
            assets = rawData.Select(data => data.ToAsset<TAsset>()).ToList();

            isApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            onCallingEvent.InvokeOnSuccessEvent();
            OnGetRealtimeAssetAction?.Invoke(assets);
        }

        #region Static 事件
        /// 即時資料列表取得事件
        /// </summary>
        public static Action<List<TAsset>> OnGetRealtimeAssetAction;
        #endregion
    }

    /// <summary>
    /// WebAPI 即時資料處理基底類別 (For僅有單一項Tag的資料項)
    /// </summary>
    public abstract class WebApiRealtimeDataHandlerBase_SingleTag : WebApiRealtimeDataHandlerBase<RealtimeAsset_SingleTag>
    {
    }
}
