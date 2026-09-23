using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
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
    public abstract class WebAPI_RawDataBase<TData> : IDataConverter<TData>
    {
        public abstract TData Convert();
    }

    /// <summary>
    /// WebAPI 資料呼叫與處理基底類別
    /// <para> + TRawData: 自建的WebAPI原始資料類別(繼承WebAPI_RawDataBase<TData>) </para>
    /// <para> + TData: 轉換後的資料類別(可自訂，無限制繼承) </para>
    /// </summary>
    public abstract class WebApiDataManagerBase<TRawData, TData> : SingletonMonoBehaviour<WebApiDataManagerBase<TRawData, TData>>
        where TRawData : WebAPI_RawDataBase<TData>
    {
        #region Event
        /// <summary>
        /// 取得WebApiData資料時
        /// </summary>
        public static Action<List<TData>> OnGetWebApiDataAction;
        [Label("[Events]"), SerializeField] protected OnCallbackEvent onCallingEvent = new OnCallbackEvent();
        #endregion

        #region Field
        [SerializeField, Expandable] protected WebApiRequestSO webApiRequestSO;
        [Foldout("[Data]"), SerializeField] protected TRawData[] webapi_rawData = new TRawData[0];
        [Foldout("[Data]"), SerializeField] protected List<TData> webapiData = new List<TData>();
        [Foldout("[Settings]"), SerializeField, Tooltip("是否只從Json節點中取得資料")] protected bool getJsonFromNode = false;
        [Foldout("[Settings]"), SerializeField, ShowIf("getJsonFromNode")] protected string jsonNodePath = "devices";

        /// <summary>
        /// WebAPI轉換後的資料
        /// </summary>
        public static List<TData> WebApiData => Instance.webapiData;
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
        public void InvokeData() => OnGetWebApiDataAction?.Invoke(webapiData);
        #endregion

        protected void OnSuccess(string json)
        {
            isWebApiCalling = false;
            webapi_rawData = new TRawData[0];
            webapiData = new List<TData>();
            ParseJson(json);
            onCallingEvent?.InvokeOnCallingEvent(isWebApiCalling);
            onCallingEvent?.InvokeOnSuccessEvent();
            InvokeData();
        }

        protected void OnFailure(string errorMsg)
        {
            isWebApiCalling = false;
            onCallingEvent?.InvokeOnCallingEvent(isWebApiCalling);
            onCallingEvent?.InvokeOnErrorEvent(errorMsg);
        }

        /// <summary>
        /// 解析Json字串資料
        /// </summary>
        public void ParseJson(string json)
        {
            if (getJsonFromNode)
            {
                json = JsonHelper.GetJsonFromNode(json, jsonNodePath);
            }
            webapi_rawData = JsonConvert.DeserializeObject<TRawData[]>(json);
            webapiData = webapi_rawData.Select(data => data.Convert()).ToList();
        }
    }
}
