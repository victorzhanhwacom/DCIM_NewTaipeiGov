using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - CCTV
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataCCTV : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_CCTV>> OnGetCCTVDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_CCTV> WebAPI_CCTVData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_CCTVData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("cctv"))
                .Select(data => data.ToCCTV()).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetCCTVDataAction?.Invoke(WebAPI_CCTVData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - CCTV
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_CCTV : WebAPI_RealtimeData
    {
        public string severity => statusTag.severity;

        /// <summary>
        /// 即時狀態Tag
        /// </summary>
        private Tag statusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":Status"));

    }
}
