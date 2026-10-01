using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 消防系統FS
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataFS : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_FS>> OnGetFSDataAction;
        public static Action<List<WebAPI_RealtimeData_GasCylinder>> OnGetGasCylinderDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_FS> WebAPI_FSData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_GasCylinder> WebAPI_GasCylinderData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_FSData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("fir"))
                .Select(data => data.ToFS()).ToList();
            WebAPI_GasCylinderData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("gas"))
                .Select(data => data.ToGasCylinder()).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetFSDataAction?.Invoke(WebAPI_FSData);
            OnGetGasCylinderDataAction?.Invoke(WebAPI_GasCylinderData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 消防系統FS
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_FS : WebAPI_RealtimeData
    {
        /// <summary>
        /// 控制盤故障警報
        /// </summary>
        public Tags controlBoardTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":conalm"));
        /// <summary>
        /// 消防第一階段警報
        /// </summary>
        public Tags level1Tag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":p1alm"));
        /// <summary>
        /// 消防第二階段警報
        /// </summary>
        public Tags level2Tag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":p2alm"));
        /// <summary>
        /// 極早期Tag
        /// </summary>
        public Tags vesdaTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":vealm"));
          /// <summary>
        /// 極早期設備故障警報
        /// </summary>
        public Tags vesdaDeviceTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":vedalm"));

        override public int TotalAlertLevelStatus => GetTotalAlertLevelStatus(controlBoardTag, level1Tag, level2Tag, vesdaTag, vesdaDeviceTag);
    }

    /// <summary>
    /// WebAPI即時資料 - 消防鋼瓶系統
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_GasCylinder : WebAPI_RealtimeData
    {
        override public int TotalAlertLevelStatus => 0;
    }
}
