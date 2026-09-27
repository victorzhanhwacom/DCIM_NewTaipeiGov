using System;
using System.Linq;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 即時溫濕度
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataRTRH : WebAPI_CallerBase<WebAPI_RealtimeData_RtRh>
    {
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
        public Tags rtTag => tags?.FirstOrDefault(tag => tag.tagId.Contains("dbt"));
        /// <summary>
        /// 即時濕度
        /// </summary>
        public Tags rhTag => tags?.FirstOrDefault(tag => tag.tagId.Contains("rh"));
    }
}
