using System;
using UnityEngine;
using static VzDev.RealTimeDataDTO;

namespace VzDev
{
    /// <summary>
    /// WebAPI 即時資料處理基底類別
    /// </summary>
    public class WebApiRealtimeDataHandler_WLK : WebApiRealtimeDataHandlerBase<RealtimeAsset_WaterLeak>
    {
    }

    /// <summary>
    /// 從WebAPI轉換過來的即時資料格式(漏水帶)
    /// </summary>
    [Serializable]
    public class RealtimeAsset_WaterLeak : RealtimeAsset
    {
        [field: SerializeField]
        public bool isLeak { get; private set; }
        [field: SerializeField]
        public string status{ get; private set; }

        [field: SerializeField]
        public int alertStatus { get; private set; }

        public override void SetTags(Tags[] tags)
        {
            base.SetTags(tags);
            isLeak = tags[0].alertLevel != 0;
            status = tags[0].value;

            alertStatus = tags[0].value == "警報" ? 2 : 0;
        }
    }
}
