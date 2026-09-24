using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.EventUtils;
using VzDev.Frameworks;
using VzDev.StringUtils;

namespace VzDev.NetUtils.WebAPI
{
    /// <summary>
    /// WebAPI原始資料基底類別
    /// </summary>
    public abstract class WebAPI_RawDataBase
    {
    }

    /// <summary>
    /// WebAPI 資料呼叫
    /// <para> + TRawData: 自建的WebAPI原始資料類別(繼承WebAPI_RawDataBase<TData>) </para>
    /// </summary>
    public abstract class WebAPI_CallerBase<TRawData> : SingletonMonoBehaviour<WebAPI_CallerBase<TRawData>>
        where TRawData : WebAPI_RawDataBase
    {
        #region Event
        /// <summary>
        /// 取得WebApiData資料時
        /// </summary>
        public static Action<List<TRawData>> OnGetRawDataAction;
        [Label("[Events]"), SerializeField] protected OnCallbackEvent onCallingEvent = new OnCallbackEvent();
        #endregion

        #region Field
        [SerializeField, Expandable] protected WebApiRequestSO webApiRequestSO;
        [Foldout("[Data]"), SerializeField] protected List<TRawData> webapi_rawData = new List<TRawData>();
        [Foldout("[Settings]"), SerializeField, Tooltip("是否只從Json節點中取得資料")] protected bool getJsonFromNode = false;
        [Foldout("[Settings]"), SerializeField, ShowIf("getJsonFromNode")] protected string jsonNodePath = "devices";

        /// <summary>
        /// WebAPI轉換後的資料
        /// </summary>
        public static List<TRawData> WebAPI_RawData => Instance.webapi_rawData;
        protected bool isHaveRequest => webApiRequestSO != null;
        protected bool isWebApiCalling;
        #endregion

        #region 呼叫WebAPI取得即時資料
        /// <summary>
        /// 呼叫WebAPI取得即時資料
        /// </summary>
        [Button, ShowIf("isHaveRequest"), DisableIf("isApiCalling")]
        public void CallWebAPI() => CallWebAPI(OnSuccess, OnFailure);
        /// <summary>
        /// 呼叫WebAPI取得即時資料
        /// </summary>
        public static void CallWebAPI_Static(Action<string> onSuccess, Action<string> onFailure)
            => Instance.CallWebAPI(onSuccess, onFailure);
        /// <summary>
        /// 呼叫WebAPI取得即時資料
        /// </summary>
        public void CallWebAPI(Action<string> onSuccess, Action<string> onFailure)
        {
            isWebApiCalling = true;
            onCallingEvent?.InvokeOnCallingEvent(isWebApiCalling);
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
            isWebApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isWebApiCalling);
            webApiRequestSO.StopCallApi();
        }
        #endregion

        #region 發送WebApiData資料給訂閱者
        /// <summary>
        /// 發送WebApiData資料給訂閱者
        /// </summary>
        public static void InvokeData_Static() => Instance.InvokeData();
        /// <summary>
        /// 發送WebApiData資料給訂閱者
        /// </summary>
        public void InvokeData() => OnGetRawDataAction?.Invoke(webapi_rawData);
        #endregion

        /// <summary>
        /// 解析Json字串資料
        /// </summary>
        public void ParseJson(string json)
        {
            if (getJsonFromNode) json = JsonHelper.GetJsonFromNode(json, jsonNodePath);
            webapi_rawData = JsonConvert.DeserializeObject<List<TRawData>>(json);
        }

        #region WebAPI呼叫時的回調
        /// <summary>
        /// 呼叫WebAPI成功時的回調
        /// </summary>
        protected void OnSuccess(string json)
        {
            isWebApiCalling = false;
            webapi_rawData = new List<TRawData>();
            ParseJson(json);
            onCallingEvent?.InvokeOnCallingEvent(isWebApiCalling);
            onCallingEvent?.InvokeOnSuccessEvent();
            InvokeData();
        }
        /// <summary>
        /// 呼叫WebAPI失敗時的回調
        /// </summary>
        protected void OnFailure(string errorMsg)
        {
            isWebApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isWebApiCalling);
            onCallingEvent?.InvokeOnErrorEvent(errorMsg);
        }
        #endregion
    }
}
