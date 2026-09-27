using System;
using System.Linq;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 即時電力狀態
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataPower : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 即時UPS主機狀態
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_UPSHost : WebAPI_RealtimeData
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 即時UPS電池狀態
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_UPSBattery : WebAPI_RealtimeData
    {
        /// <summary>
        /// 單體電壓
        /// </summary>
        public Tags voltage => tags?.FirstOrDefault(tag => tag.tagId.Contains("indv"));
        /// <summary>
        /// 單體內阻
        /// </summary>
        public Tags irTag => tags?.FirstOrDefault(tag => tag.tagId.Contains("minr"));
        /// <summary>
        /// 單體告警
        /// </summary>
        public Tags alarmTag => tags?.FirstOrDefault(tag => tag.tagId.Contains("indalm"));
    }
}
