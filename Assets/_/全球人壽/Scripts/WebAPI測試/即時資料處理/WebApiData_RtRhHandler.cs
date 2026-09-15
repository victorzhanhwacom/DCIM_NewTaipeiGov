using System;
using System.Linq;
using UnityEngine;
using static VzDev.RealTimeDataDTO;

namespace VzDev
{
    /// <summary>
    /// 全球人壽 溫濕度資料
    /// </summary>
    public class WebApiData_RtRhHandler : WebApiRealtimeDataHandlerBase<RealtimeAsset_RtRh>
    {
    }

    /// <summary>
    /// 從WebAPI轉換過來的即時資料格式(溫濕度)
    /// </summary>
    [Serializable]
    public class RealtimeAsset_RtRh : RealtimeAsset
    {
        [field: SerializeField]
        public Tags rtTag { get; private set; }
        [field: SerializeField]
        public Tags rhTag { get; private set; }

        public override void SetTags(Tags[] tags)
        {
            base.SetTags(tags);
            rtTag = rawTags.FirstOrDefault(tag => tag.name == "dbt");
            rhTag = rawTags.FirstOrDefault(tag => tag.name == "rh");
        }
    }
}
