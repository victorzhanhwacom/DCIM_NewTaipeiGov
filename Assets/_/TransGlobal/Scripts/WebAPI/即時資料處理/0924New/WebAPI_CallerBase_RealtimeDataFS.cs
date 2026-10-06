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
            /*  WebAPI_GasCylinderData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("gas"))
                 .Select(data => data.ToGasCylinder()).ToList(); */
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetFSDataAction?.Invoke(WebAPI_FSData);
        }

        public static void SetGasCylinderData(List<WebAPI_RealtimeData_GasCylinder> gasCylinderData)
        {
            WebAPI_GasCylinderData = gasCylinderData;
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
        public Tag controlBoardTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":conalm"));
        /// <summary>
        /// 消防第一階段警報
        /// </summary>
        public Tag level1Tag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":p1alm"));
        /// <summary>
        /// 消防第二階段警報
        /// </summary>
        public Tag level2Tag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":p2alm"));
        /// <summary>
        /// 極早期Tag
        /// </summary>
        public Tag vesdaTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":vealm"));
        /// <summary>
        /// 極早期設備故障警報
        /// </summary>
        public Tag vesdaDeviceTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":vedalm"));
    }

    /// <summary>
    /// WebAPI即時資料 - 消防鋼瓶系統
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_GasCylinder : WebAPI_RealtimeData
    {
        public string displayName => tags[0].displayName;
        public string message => tags[0].message;
    }
}
