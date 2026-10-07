using System.Collections.Generic;
using System.Linq;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 機櫃門禁
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataRackDoor : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Field
        public static List<WebAPI_RealtimeData_RackDoor> WebAPI_RackDoorData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_RackDoorData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("EL"))
                .Select(data => data.ToRackDoor()).ToList();
            WebAPI_CallerBase_RealtimeDataDoor.SetRackDoorData(WebAPI_RackDoorData);
        }
    }
}
