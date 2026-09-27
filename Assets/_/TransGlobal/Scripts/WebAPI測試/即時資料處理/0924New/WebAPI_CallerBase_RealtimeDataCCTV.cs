using System;
using System.Linq;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - CCTV
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataCCTV : WebAPI_CallerBase<WebAPI_RealtimeData_CCTV>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - CCTV
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_CCTV : WebAPI_RealtimeData
    {
        /// <summary>
        /// 即時狀態Tag
        /// </summary>
        public Tags statusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("Status"));
    }
}
