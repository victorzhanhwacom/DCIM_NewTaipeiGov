using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using VzDev.Frameworks;
using VzDev.StringUtils;

namespace VzDev
{
    /// <summary>
    /// 全球人壽 溫濕度資料
    /// </summary>
    public class WebApiData_RtRhHandler : SingletonMonoBehaviour<WebApiData_RtRhHandler>
    {
        [SerializeField] private RealTimeDataDTO[] realTimeData;
        [SerializeField] private RealtimeAsset_RtRh[] rtRhDatas;

        public void ParseJson(string json)
        {
            realTimeData = new RealTimeDataDTO[0];
            rtRhDatas = new RealtimeAsset_RtRh[0];

            json = JsonHelper.GetJsonFromNode(json, "devices");
            realTimeData = JsonConvert.DeserializeObject<RealTimeDataDTO[]>(json);
            rtRhDatas = realTimeData.Select(data => data.ToAsset<RealtimeAsset_RtRh>()).ToArray();
        }
    }
}
