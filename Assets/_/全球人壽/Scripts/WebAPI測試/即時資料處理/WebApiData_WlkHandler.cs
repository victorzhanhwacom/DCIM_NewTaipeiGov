using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.Frameworks;
using VzDev.StringUtils;

namespace VzDev
{
    /// <summary>
    /// 全球人壽 漏水資料
    /// </summary>
    public class WebApiData_WlkHandler : SingletonMonoBehaviour<WebApiData_WlkHandler>
    {
        [SerializeField] private RealTimeDataDTO[] realTimeData;
        [SerializeField] private RealtimeAsset_Wlk[] wlkAssets;

        public void ParseJson(string json)
        {
            realTimeData = new RealTimeDataDTO[0];
            wlkAssets = new RealtimeAsset_Wlk[0];

            json = JsonHelper.GetJsonFromNode(json, "devices");
            realTimeData = JsonConvert.DeserializeObject<RealTimeDataDTO[]>(json);
            wlkAssets = realTimeData.Select(data => data.ToAsset<RealtimeAsset_Wlk>()).ToArray();
        }
    }
}
