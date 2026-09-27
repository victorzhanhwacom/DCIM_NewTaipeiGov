using System;

namespace VzDev
{
    /// <summary>
    /// WebAPI 即時資料呼叫 - InRowCooler
    /// </summary>
    public class WebApiRealtimeDataHandler_InRowCooler : WebApiRealtimeDataHandlerBase<RealtimeAsset_UpsBattery>
    {
    }

    /// <summary>
    /// 從WebAPI轉換過來的即時資料格式(UPS電池)
    /// </summary>
    [Serializable]
    public class RealtimeAsset_InRowCooler : WebApi_RealtimeData
    {

    }

}
