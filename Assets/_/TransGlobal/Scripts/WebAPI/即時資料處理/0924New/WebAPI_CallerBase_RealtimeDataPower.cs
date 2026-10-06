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
        public static Action<List<WebAPI_RealtimeData_PowerPanel>> OnGetPowerPanelDataAction;
        public static Action<List<WebAPI_RealtimeData_RackPower>> OnGetRackPowerDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_UPSHost> WebAPI_UpsHostData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_UPSBattery> WebAPI_UpsBatteryData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_PowerPanel> WebAPI_PowerPanelData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_RackPower> WebAPI_RackPowerData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            webapiData?.Sort((a, b) => a.deviceName.CompareTo(b.deviceName));

            WebAPI_UpsHostData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("ups"))
                .Select(data => data.ToUPSHost()).ToList();
            WebAPI_UpsBatteryData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("bat"))
                .Select(data => data.ToUPSBattery()).ToList();

            WebAPI_PowerPanelData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("pm"))
                .Select(data => data.CloneAs<WebAPI_RealtimeData_PowerPanel>()).ToList();

            WebAPI_RackPowerData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("ecm"))
                .Select(data => data.CloneAs<WebAPI_RealtimeData_RackPower>()).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetUpsHostDataAction?.Invoke(WebAPI_UpsHostData);
            OnGetUpsBatteryDataAction?.Invoke(WebAPI_UpsBatteryData);
            OnGetPowerPanelDataAction?.Invoke(WebAPI_PowerPanelData);
            OnGetRackPowerDataAction?.Invoke(WebAPI_RackPowerData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 即時UPS主機狀態
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_UPSHost : WebAPI_RealtimeData
    {
        /// <summary>
        /// 目前三相總實際輸出功率 (總輸出功率)
        /// </summary>
        public Tag OutputTotalWattTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":ai014"));

        /// <summary>
        /// 運行模式-電池模式
        /// </summary>
        public Tag BatteryModeTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":di001"));
    }

    /// <summary>
    /// WebAPI即時資料 - 即時UPS電池狀態
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_UPSBattery : WebAPI_RealtimeData
    {
        /// <summary>
        /// 單體內阻
        /// </summary>
        public Tag irTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":minr"));
        /// <summary>
        /// 單體電壓
        /// </summary>
        public Tag voltageTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":indv"));
        /// <summary>
        /// 單體告警
        /// </summary>
        public Tag alarmTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":indalm"));
    }

    /*
    I_R	R 相電流	A（安培）	第一相的電流
    I_S	S 相電流	A	第二相的電流
    I_T	T 相電流	A	第三相的電流
    kW	有效功率（實功率）	kW	目前的即時總耗電功率，通常是三相加總
    kWh	累積電能（用電度數）	kWh（度）	從電表啟用或歸零起累計的用電量，會持續遞增
    PF_R	R 相功率因數	無單位	Power Factor，範圍約 0～1（或 -1～1）
    PF_S	S 相功率因數	無單位	同上
    PF_T	T 相功率因數	無單位	同上
    Vavg	平均電壓	V（伏特）	三相電壓的平均值
     */
    /// <summary>
    /// WebAPI即時資料 - 配電盤
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_PowerPanel : WebAPI_RealtimeData
    {
        /// <summary>
        /// 第一相的電流
        /// </summary>
        public Tag i_rTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":ir"));
        /// <summary>
        /// 第二相的電流
        /// </summary>
        public Tag i_sTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":is"));
        /// <summary>
        /// 第三相的電流
        /// </summary>
        public Tag i_tTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":it"));
        /// <summary>
        /// 有效功率
        /// </summary>
        public Tag kWTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":kw"));
        /// <summary>
        /// 累積電能
        /// </summary>
        public Tag kWHTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":kwh"));
        /// <summary>
        /// R 相功率因數
        /// </summary>
        public Tag pf_rTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":pfr"));
        /// <summary>
        /// S 相功率因數
        /// </summary>
        public Tag pf_sTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":pfs"));
        /// <summary>
        /// T 相功率因數
        /// </summary>
        public Tag pf_tTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":pft"));
        /// <summary>
        /// 平均電壓
        /// </summary>
        public Tag vavgTag => tags?.FirstOrDefault(tag => tag.tagId.Contains(":vavg"));
    }

    /// <summary>
    /// WebAPI即時資料 - 機櫃電源
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_RackPower : WebAPI_RealtimeData
    {
    }
}
