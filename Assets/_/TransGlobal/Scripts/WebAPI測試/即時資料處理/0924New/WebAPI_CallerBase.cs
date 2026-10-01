using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using VzDev.EventUtils;
using VzDev.Frameworks;
using VzDev.StringUtils;

namespace VzDev.NetUtils.WebAPI
{
    /// <summary>
    /// WebAPI 資料呼叫 抽像基底類別
    /// <para> + TData: WebAPI資料類別  </para>
    /// </summary>
    public abstract class WebAPI_CallerBase<TData> : SingletonMonoBehaviour<WebAPI_CallerBase<TData>>
        where TData : class
    {
        #region Event
        /// <summary>
        /// 取得WebApiData資料時
        /// </summary>
        public static Action<List<TData>> OnGetDataAction;
        [Foldout("[Data Event]")] public UnityEvent<List<TData>> OnGetDataEvent;
        [Label("[Events]")] public OnCallbackEvent onCallingEvent = new OnCallbackEvent();
        #endregion

        #region Field
        [SerializeField, Expandable] protected WebApiRequestSO webApiRequestSO;
        [SerializeField] protected List<TData> webapiData = new List<TData>();
        [Foldout("[Settings]"), SerializeField, Tooltip("是否只從Json節點中取得資料")] protected bool getJsonFromNode = true;
        [Foldout("[Settings]"), SerializeField, ShowIf("getJsonFromNode")] protected string jsonNodePath = "devices";

        /// <summary>
        /// WebAPI轉換後的資料
        /// </summary>
        public static List<TData> WebAPI_RawData => Instance.webapiData;
        protected bool isHaveRequest => webApiRequestSO != null;
        protected bool isWebApiCalling;
        #endregion

        public void ToCallWebAPI(bool isCall)
        {
            if(isCall) CallWebAPI();
            else StopCallApi();
        }

        public void RecallWebAPI() => CallWebAPI(OnSuccess, OnFailure);

        #region 呼叫WebAPI取得即時資料
        /// <summary>
        /// 呼叫WebAPI取得即時資料
        /// </summary>
        [Button, ShowIf("isHaveRequest"), DisableIf("isWebApiCalling")]
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
            webapiData?.Clear();
            webapiData ??= new List<TData>();
            onCallingEvent?.InvokeStartEvent();
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
        [Button, ShowIf("isWebApiCalling")]
        public void StopCallApi()
        {
            isWebApiCalling = false;
            onCallingEvent?.InvokeStopCallEvent();
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
        public virtual void InvokeData()
        {
            OnGetDataAction?.Invoke(webapiData);
            OnGetDataEvent?.Invoke(webapiData);
        }
        #endregion

        /// <summary>
        /// 解析Json字串資料
        /// </summary>
        public virtual void ParseJson(string json)
        {
            if (getJsonFromNode) json = JsonHelper.GetJsonFromNode(json, jsonNodePath);
            webapiData = JsonConvert.DeserializeObject<List<TData>>(json);
        }

        /// <summary>
        /// 設定Body Json內容
        /// </summary>
        public void SetBodyJson(string bodyJson) => webApiRequestSO.SetBodyRawJson(bodyJson);

        #region WebAPI呼叫時的回調
        /// <summary>
        /// 呼叫WebAPI成功時的回調
        /// </summary>
        protected void OnSuccess(string json)
        {
            isWebApiCalling = false;
            webapiData = new List<TData>();
            ParseJson(json);
            onCallingEvent?.InvokeOnSuccessEvent();
            InvokeData();
        }
        /// <summary>
        /// 呼叫WebAPI失敗時的回調
        /// </summary>
        protected void OnFailure(string errorMsg)
        {
            isWebApiCalling = false;
            onCallingEvent?.InvokeOnFaliureEvent(errorMsg);
        }
        #endregion

        /// <summary>
        /// 呼叫逾時處理
        /// </summary>
        public void OnTimeout()
        {
            
        }
    }
}