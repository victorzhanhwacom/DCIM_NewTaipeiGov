using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 消防鋼瓶
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataFS_GasCylinder : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
       #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_GasCylinder> WebAPI_GasCylinderData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_GasCylinderData = webapiData?.Where(data => data.tags?.Any(tag => tag.displayName.Contains("鋼瓶")) == true)
                .Select(data => data.ToGasCylinder()).ToList();

            WebAPI_CallerBase_RealtimeDataFS.SetGasCylinderData(WebAPI_GasCylinderData);
        }
    }
}
