using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 空調系統HVAC
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataHVAC : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_CRAC_MainController>> OnGetCRAC_MainControllerDataAction;
        public static Action<List<WebAPI_RealtimeData_CRAC>> OnGetCRACDataAction;
        public static Action<List<WebAPI_RealtimeData_InRowCooler>> OnGetInRowCoolerDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_CRAC_MainController> WebAPI_CRAC_MainControllerData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_CRAC> WebAPI_CRACData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_InRowCooler> WebAPI_InRowCoolerData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);

            WebAPI_CRAC_MainControllerData = webapiData?.Where(data => data.deviceCategory == "fcu_s")
               .Select(data => data.CloneAs<WebAPI_RealtimeData_CRAC_MainController>()).ToList();

            //若deviceName包含"UPS機房"，則將UPS機房取代為電力室
            foreach (var data in WebAPI_CRAC_MainControllerData)
            {
                if (data.deviceName.Contains("UPS機房"))
                {
                    data.SetDeviceName(data.deviceName.Replace("UPS機房", "電力室"));
                }
            }

            WebAPI_CRACData = webapiData?.Where(data => data.deviceCategory == "fcu")
                .Select(data => data.CloneAs<WebAPI_RealtimeData_CRAC>()).ToList();
            // 以TotalAlertLevelStatus排序，將有告警的設備排在前面
            WebAPI_CRACData = WebAPI_CRACData?.OrderByDescending(data => data.TotalAlertLevelStatus).ToList();

            // 若tempSetTag.value為"---"，則將tempSetTag.value設為rtTag.value
            CheckAndSet_CRAC_TempSet();

            WebAPI_InRowCoolerData = webapiData?.Where(data => data.deviceCategory == "inr")
                .Select(data => data.CloneAs<WebAPI_RealtimeData_InRowCooler>()).ToList();
            // 以TotalAlertLevelStatus排序，將有告警的設備排在前面
            WebAPI_InRowCoolerData = WebAPI_InRowCoolerData?.OrderByDescending(data => data.TotalAlertLevelStatus).ToList();
        }
        private void CheckAndSet_CRAC_TempSet()
        {
            foreach (var cracData in WebAPI_CRACData)
            {
                if (cracData.tempSetTag.value == "---")
                {
                    // 將cracData.rtTag.value浮點數為string轉為int，並將其設為cracData.tempSetTag.value
                    if (float.TryParse(cracData.rtTag.value, out float rtValue))
                    {
                        cracData.tempSetTag.SetValue(Mathf.RoundToInt(rtValue).ToString());
                    }
                }
            }
        }

        public override void InvokeData()
        {
            base.InvokeData();
            OnGetCRAC_MainControllerDataAction?.Invoke(WebAPI_CRAC_MainControllerData);
            OnGetCRACDataAction?.Invoke(WebAPI_CRACData);
            OnGetInRowCoolerDataAction?.Invoke(WebAPI_InRowCoolerData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 空調箱 中控總開關
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_CRAC_MainController : WebAPI_RealtimeData
    {
        public string controllRoom => deviceName.Split("_")[0];
        public bool IsAutoControlMode => tags[0]?.value == "自動模式";

        /// <summary>
        /// 中控控制對像: 空調箱CRAC列表
        /// </summary>
        public List<WebAPI_RealtimeData_CRAC> cracList {get; private set;} = new List<WebAPI_RealtimeData_CRAC>();

        public void AddCRAC(WebAPI_RealtimeData_CRAC crac)
        {
            if (crac == null) return;
            if (cracList == null) cracList = new List<WebAPI_RealtimeData_CRAC>();
            cracList.Add(crac);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 空調箱CRAC
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_CRAC : WebAPI_RealtimeData
    {
        public string controllRoom => deviceName.Split("_")[0];
        /// <summary>
        /// 室溫
        /// </summary>
        public Tag rtTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":ts"));
        /// <summary>
        /// 手動啟動
        /// </summary>
        public Tag controlTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":onf"));
        /// <summary>
        /// 電源狀態Tag: 關機, 開機
        /// </summary>
        public Tag powerStatusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":run"));
        /// <summary>
        /// 設定溫度Tag
        /// </summary>
        public Tag tempSetTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":tss"));
        /// <summary>
        /// 告警狀態Tag
        /// </summary>
        public Tag alarmTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":trip"));

        /// <summary>
        /// 手動啟動狀態: 啟動(true), 停止(false)
        /// </summary>
        public bool manualControlStatus => controlTag.value == "啟動";
        /// <summary>
        /// 電源狀態: 開機(true), 關機(false)
        /// </summary>
        public bool powerStatus => powerStatusTag.value == "開機";
        /// <summary>
        /// 告警狀態: 0:無告警,1:高溫告警,2:感溫器故障,3:記憶體故障
        /// </summary>
        public int AlarmStatus
        {
            get
            {
                switch (alarmTag.value)
                {
                    case "無告警": return 0;
                    case "高溫告警": return 1;
                    case "感溫器故障": return 2;
                    case "記憶體故障": return 3;
                    default: return 0;
                }
            }
        }
    }

    /// <summary>
    /// WebAPI即時資料 - InRowCooler
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_InRowCooler : WebAPI_RealtimeData
    {
        /// <summary>
        /// 回風溫度
        /// </summary>
        public Tag inTempTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":ai002"));
        /// <summary>
        /// 出風溫度
        /// </summary>
        public Tag outTempTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":ai001"));

    }
}
