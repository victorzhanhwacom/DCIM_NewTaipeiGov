using System;
using System.Linq;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 空調系統HVAC
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataHVAC : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 空調箱CRAC
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_CRAC : WebAPI_RealtimeData
    {
        /// <summary>
        /// 手動操控Tag
        /// </summary>
        public Tags controlTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("onf"));
        /// <summary>
        /// 電源狀態Tag: 關機, 開機
        /// </summary>
        public Tags statusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("run"));
        /// <summary>
        /// 告警狀態Tag
        /// </summary>
        public Tags alarmTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("trip"));
        /// <summary>
        /// 室溫Tag
        /// </summary>
        public Tags rtTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("ts"));
        /// <summary>
        /// 設定溫度Tag
        /// </summary>
        public Tags tempSetTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("tss"));
    }

    /// <summary>
    /// WebAPI即時資料 - InRowCooler
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_InRowCooler : WebAPI_RealtimeData
    {
        /// <summary>
        /// 出風溫度
        /// </summary>
        public Tags outTempTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("ai001"));
        /// <summary>
        /// 回風溫度
        /// </summary>
        public Tags returnTempTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("ai002"));
    }
}
