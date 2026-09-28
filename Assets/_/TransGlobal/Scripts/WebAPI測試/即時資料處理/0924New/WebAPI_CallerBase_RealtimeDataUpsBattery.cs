using System;
using System.Linq;
using UnityEngine;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - UPS電池
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataUpsBattery : WebAPI_CallerBase<WebAPI_RealtimeData_UPSBattery>
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
        public Tags voltageTag => tags?.FirstOrDefault(tag => tag.tagId.Contains("indv"));
        /// <summary>
        /// 單體內阻
        /// </summary>
        public Tags irTag => tags?.FirstOrDefault(tag => tag.tagId.Contains("minr"));
        /// <summary>
        /// 單體告警
        /// </summary>
        public Tags alarmTag => tags?.FirstOrDefault(tag => tag.tagId.Contains("indalm"));

        /// <summary>
        /// 即時告警 0:正常 1:告警 99:斷線
        /// </summary>
        public int alertLevel
        {
            get
            {
                float result = Mathf.Max(voltageTag.alertLevel, irTag.alertLevel, alarmTag.alertLevel);
                if (result == 0) return 0;
                else if (result == 99) return 2;
                else return 1;
            }
        }
    }
}
