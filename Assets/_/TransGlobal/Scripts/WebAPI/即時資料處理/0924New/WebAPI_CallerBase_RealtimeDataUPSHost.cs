using System.Collections.Generic;
using System.Linq;
using VzDev.NetLibrary.Extensions;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - UPS主機即時資料 (資料回傳較慢，所以獨立出來)
    /// <para> + 取得資料並解析後，將資料傳給 WebAPI_CallerBase_RealtimeDataPower來Invoke發送</para>
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataUPSHost : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Field
        private List<WebAPI_RealtimeData_UPSHost> WebAPI_UpsHostData;
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_UpsHostData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("ups"))
                .Select(data => data.ToUPSHost()).ToList();

#if UNITY_EDITOR
            //依deviceName排序，其tag依照tagId排序
            webapiData?.Sort((a, b) => a.deviceName.CompareTo(b.deviceName));
#endif
        }
        public override void InvokeData()
        {
            base.InvokeData();
            WebAPI_CallerBase_RealtimeDataPower.SetUPSHostData(WebAPI_UpsHostData);
        }
    }
}
