using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 漏水帶
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataWaterLeak : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_WaterLeak>> OnGetWaterLeakDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_WaterLeak> WebAPI_WaterLeakData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_WaterLeakData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("lea"))
                .Select(data => data.ToWaterLeak()).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetWaterLeakDataAction?.Invoke(WebAPI_WaterLeakData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 漏水帶
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_WaterLeak : WebAPI_RealtimeData
    {
        public string value => alarmTag?.value;
        public string severity => alarmTag.severity;
        /// <summary>
        /// 漏水警報
        /// </summary>
        private Tags alarmTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":alarm"));

        override public int TotalAlertLevelStatus => alarmTag?.alertLevelStatus ?? 0;
    }
}
