using System;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 消防系統FS
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataFS : WebAPI_CallerBase<WebAPI_RealtimeData_FS>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 消防系統FS
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_FS : WebAPI_RealtimeData
    {
        /// <summary>
        /// 消防第一階段警報
        /// </summary>
        public Tags firstAlarmTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("p1alm"));
        /// <summary>
        /// 消防第二階段警報
        /// </summary>
        public Tags secondAlarmTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("p2alm"));
        /// <summary>
        /// 極早期Tag
        /// </summary>
        public Tags vesdaTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("vealm"));
        /// <summary>
        /// 控制盤Tag
        /// </summary>
        public Tags controlBoardTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("conalm"));
        /// <summary>
        /// "消防氣體噴發Tag
        /// </summary>
        public Tags sprayGasTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("fgealm"));
    }
}
