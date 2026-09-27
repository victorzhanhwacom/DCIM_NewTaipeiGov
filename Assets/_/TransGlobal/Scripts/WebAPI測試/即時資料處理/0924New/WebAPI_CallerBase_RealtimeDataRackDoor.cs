using System;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 機櫃門
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataRackDoor : WebAPI_CallerBase<WebAPI_RealtimeData_RackDoor>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 機櫃門
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_RackDoor : WebAPI_RealtimeData
    {
    }
}
