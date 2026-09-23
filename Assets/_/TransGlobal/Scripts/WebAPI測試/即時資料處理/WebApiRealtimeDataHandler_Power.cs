using System;

namespace VzDev
{
    /// <summary>
    /// 全球人壽 能源資料
    /// </summary>
    public class WebApiRealtimeDataHandler_Power : WebApiRealtimeDataHandlerBase<RealtimeAsset_UpsBattery>
    {
    }

    /// <summary>
    /// 從WebAPI轉換過來的即時資料格式(UPS電池)
    /// </summary>
    [Serializable]
    public class RealtimeAsset_UpsBattery : RealtimeAsset
    {

    }

}
