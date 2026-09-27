using System;
using System.Linq;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 漏水帶
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataWaterLeak : WebAPI_CallerBase<WebAPI_RealtimeData_WaterLeak>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 漏水帶
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_WaterLeak : WebAPI_RealtimeData
    {
        /// <summary>
        /// 漏水警報
        /// </summary>
        public Tags alarmTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("alarm"));
    }
}
