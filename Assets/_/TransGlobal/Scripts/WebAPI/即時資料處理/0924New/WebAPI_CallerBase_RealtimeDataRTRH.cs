using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.NetLibrary.Extensions;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 即時溫濕度
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataRTRH : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_RtRh>> OnGetRtRhDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_RtRh> WebAPI_RtRhData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_RtRhData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("ia"))
                .Select(data => data.ToRtRh()).ToList();

                // 以TotalAlertLevelStatus排序，將有告警的設備排在前面
            WebAPI_RtRhData = WebAPI_RtRhData?.OrderByDescending(data => data.TotalAlertLevelStatus).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetRtRhDataAction?.Invoke(WebAPI_RtRhData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 溫濕度
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_RtRh : WebAPI_RealtimeData
    {
        /// <summary>
        /// 即時溫度
        /// </summary>
        public Tag rtTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":dbt"));
        /// <summary>
        /// 即時濕度
        /// </summary>
        public Tag rhTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":rh"));
    }
}
