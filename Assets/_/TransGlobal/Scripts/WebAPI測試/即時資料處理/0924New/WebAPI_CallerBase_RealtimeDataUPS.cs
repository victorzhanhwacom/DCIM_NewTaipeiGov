using System;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - UPS主機
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataUps : WebAPI_CallerBase<WebAPI_RealtimeData_UPSHost>
    {
    }

    /// <summary>
    /// WebAPI即時資料 - 即時UPS主機狀態
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_UPSHost : WebAPI_RealtimeData
    {
    }
}
