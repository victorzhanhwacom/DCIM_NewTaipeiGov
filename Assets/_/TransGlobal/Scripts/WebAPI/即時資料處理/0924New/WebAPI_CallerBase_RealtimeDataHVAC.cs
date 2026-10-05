using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 空調系統HVAC
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataHVAC : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_CRAC>> OnGetCRACDataAction;
        public static Action<List<WebAPI_RealtimeData_InRowCooler>> OnGetInRowCoolerDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_CRAC> WebAPI_CRACData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_InRowCooler> WebAPI_InRowCoolerData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_CRACData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("fcu"))
                .Select(data => data.ToCRAC()).ToList();

            // 以TotalAlertLevelStatus排序，將有告警的設備排在前面
            WebAPI_CRACData = WebAPI_CRACData?.OrderByDescending(data => data.TotalAlertLevelStatus).ToList();

            // 若tempSetTag.value為"---"，則將tempSetTag.value設為rtTag.value
            foreach (var cracData in WebAPI_CRACData)
            {
                if (cracData.tempSetTag.value == "---")
                {
                    cracData.tempSetTag.SetValue(cracData.rtTag.value);
                }
            }

            WebAPI_InRowCoolerData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("inr"))
                .Select(data => data.ToInRowCooler()).ToList();

            // 以TotalAlertLevelStatus排序，將有告警的設備排在前面
            WebAPI_InRowCoolerData = WebAPI_InRowCoolerData?.OrderByDescending(data => data.TotalAlertLevelStatus).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetCRACDataAction?.Invoke(WebAPI_CRACData);
            OnGetInRowCoolerDataAction?.Invoke(WebAPI_InRowCoolerData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 空調箱CRAC
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_CRAC : WebAPI_RealtimeData
    {
        /// <summary>
        /// 室溫
        /// </summary>
        public Tag rtTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":ts"));
        /// <summary>
        /// 手動啟動
        /// </summary>
        public Tag controlTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":onf"));
        /// <summary>
        /// 電源狀態Tag: 關機, 開機
        /// </summary>
        public Tag powerStatusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":run"));
        /// <summary>
        /// 設定溫度Tag
        /// </summary>
        public Tag tempSetTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":tss"));
        /// <summary>
        /// 告警狀態Tag
        /// </summary>
        public Tag alarmTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":trip"));

        /// <summary>
        /// 手動啟動狀態: 啟動(true), 停止(false)
        /// </summary>
        public bool manualControlStatus => controlTag.value == "啟動";
        /// <summary>
        /// 電源狀態: 開機(true), 關機(false)
        /// </summary>
        public bool powerStatus => powerStatusTag.value == "開機";
        /// <summary>
        /// 告警狀態: 0:無告警,1:高溫告警,2:感溫器故障,3:記憶體故障
        /// </summary>
        public int AlarmStatus
        {
            get
            {
                switch (alarmTag.value)
                {
                    case "無告警": return 0;
                    case "高溫告警": return 1;
                    case "感溫器故障": return 2;
                    case "記憶體故障": return 3;
                    default: return 0;
                }
            }
        }

        override public int TotalAlertLevelStatus => GetTotalAlertLevelStatus(rtTag, controlTag, powerStatusTag, tempSetTag, alarmTag);
    }

    /// <summary>
    /// WebAPI即時資料 - InRowCooler
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_InRowCooler : WebAPI_RealtimeData
    {
        /// <summary>
        /// 回風溫度
        /// </summary>
        public Tag inTempTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":ai002"));
        /// <summary>
        /// 出風溫度
        /// </summary>
        public Tag outTempTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":ai001"));

        override public int TotalAlertLevelStatus => GetTotalAlertLevelStatus(inTempTag, outTempTag);
    }
}
