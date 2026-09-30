using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.NetLibrary.Extensions;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 能源
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataPower : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_UPSHost>> OnGetUpsHostDataAction;
        public static Action<List<WebAPI_RealtimeData_UPSBattery>> OnGetUpsBatteryDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_UPSHost> WebAPI_UpsHostData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_UPSBattery> WebAPI_UpsBatteryData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_UpsHostData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("fcu"))
                .Select(data => data.ToUPSHost()).ToList();
            WebAPI_UpsBatteryData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("inr"))
                .Select(data => data.ToUPSBattery()).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetUpsHostDataAction?.Invoke(WebAPI_UpsHostData);
            OnGetUpsBatteryDataAction?.Invoke(WebAPI_UpsBatteryData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 即時UPS主機狀態
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_UPSHost : WebAPI_RealtimeData
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
