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
    /// WebAPI 資料呼叫與處理基底類別
    /// </summary>
    public abstract class WebApiRealtimeDataHandlerBase<TData> : SingletonMonoBehaviour<WebApiRealtimeDataHandlerBase<TData>>
        where TData : WebApi_RealtimeData, new()
    {
        #region Field
        [Label("[Events]"), SerializeField] protected OnCallbackEvent onCallingEvent = new OnCallbackEvent();
        [SerializeField, Expandable] protected WebApiRequestSO webApiRequestSO;
        [Foldout("[Data]"), SerializeField] protected WebApi_RealtimeDataDTO[] rawDataDTO = new WebApi_RealtimeDataDTO[0];
        [Foldout("[Data]"), SerializeField] protected List<TData> webapiData = new List<TData>();

        /// <summary>
        /// 即時資料
        /// </summary>
        public static List<TData> WebApiData => Instance.webapiData;

        protected bool isHaveRequest => webApiRequestSO != null;
        protected bool isApiCalling;

        #endregion

        #region 呼叫WebAPI取得即時資料
        /// <summary>
        /// 呼叫WebAPI取得即時資料
        /// </summary>
        public static void CallWebAPI_Static(Action<string> onSuccess, Action<string> onFailure)
            => Instance.CallWebAPI(onSuccess, onFailure);
        /// <summary>
        /// 呼叫WebAPI取得即時資料
        /// </summary>
        [Button, ShowIf("isHaveRequest"), DisableIf("isApiCalling")]
        public void CallWebAPI() => CallWebAPI(OnSuccess, OnFailed);
        /// <summary>
        /// 呼叫WebAPI取得即時資料
        /// </summary>
        public void CallWebAPI(Action<string> onSuccess, Action<string> onFailure)
        {
            isApiCalling = true;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            webApiRequestSO.CallAPI(onSuccess, onFailure);
        }
        #endregion

        #region 取消呼叫WebAPI
        /// <summary>
        /// 取消呼叫WebAPI
        /// </summary>
        public static void StopCallApi_Static() => Instance.StopCallApi();
        /// <summary>
        /// 取消呼叫WebAPI
        /// </summary>
        [Button, ShowIf("isApiCalling")]
        public void StopCallApi()
        {
            isApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            webApiRequestSO.StopCallApi();
        }
        #endregion

        private void OnSuccess(string json)
        {
            isApiCalling = false;
            ParseJson(json);
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            onCallingEvent?.InvokeOnSuccessEvent();
            OnGetWebApiDataAction?.Invoke(webapiData);
        }

        private void OnFailed(string message)
        {
            isApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isApiCalling);
            onCallingEvent?.InvokeOnErrorEvent(message);
        }


        public void ParseJson(string json)
        {
            rawDataDTO = new WebApi_RealtimeDataDTO[0];
            webapiData = new List<TData>();
            json = JsonHelper.GetJsonFromNode(json, "devices");
            rawDataDTO = JsonConvert.DeserializeObject<WebApi_RealtimeDataDTO[]>(json);
            webapiData = rawDataDTO.Select(data => data.Convert<TData>()).ToList();
        }

        #region Static 事件
        /// 取得WebApiData資料時
        /// </summary>
        public static Action<List<TData>> OnGetWebApiDataAction;
        #endregion
    }

    /// <summary>
    /// WebAPI 即時資料處理基底類別 (For僅有單一項Tag的資料項)
    /// </summary>
    public abstract class WebApiRealtimeDataHandlerBase_SingleTag : WebApiRealtimeDataHandlerBase<RealtimeAsset_SingleTag>
    {
    }
}
