using System;
using System.Linq;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 門禁
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataDoor : WebAPI_CallerBase<WebAPI_RealtimeData_Door>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 門禁
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_Door : WebAPI_RealtimeData
    {
        /// <summary>
        /// 即時狀態Tag
        /// </summary>
        public Tags statusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword("Status"));
    }
}
