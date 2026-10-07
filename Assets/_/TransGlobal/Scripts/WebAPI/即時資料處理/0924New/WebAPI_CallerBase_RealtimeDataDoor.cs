using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.DCIMUtils.Extensions;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;
using static RackDoorMap;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 門禁
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataDoor : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_Door>> OnGetDoorDataAction;
        public static Action<List<WebAPI_RealtimeData_RackDeviceCode>> OnGetRackDeviceCodeDataAction;
        #endregion

        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_Door> WebAPI_DoorData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_RackDeviceCode> WebAPI_RackDeviceCodeData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_RackDoor> WebAPI_RackDoorData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_DoorData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("AC"))
                .Select(data => data.ToDoor()).ToList();
            WebAPI_RackDeviceCodeData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("Rack"))
                .Select(data => data.CloneAs<WebAPI_RealtimeData_RackDeviceCode>()).ToList();

            if (WebAPI_RackDoorData != null)
            {
                WebAPI_RackDeviceCodeData.ForEach(rackDeviceCode => rackDeviceCode.FindRackDoorData(WebAPI_RackDoorData));
            }

            /* WebAPI_RackDoorData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("EL"))
                .Select(data => data.ToRackDoor()).ToList(); */
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetDoorDataAction?.Invoke(WebAPI_DoorData);
            OnGetRackDeviceCodeDataAction?.Invoke(WebAPI_RackDeviceCodeData);
            //OnGetRackDoorDataAction?.Invoke(WebAPI_RackDoorData);
        }

        public static void SetRackDoorData(List<WebAPI_RealtimeData_RackDoor> rackDoorData)
        {
            WebAPI_RackDoorData = rackDoorData;
            WebAPI_RackDeviceCodeData.ForEach(rackDeviceCode => rackDeviceCode.FindRackDoorData(WebAPI_RackDoorData));
            OnGetRackDeviceCodeDataAction?.Invoke(WebAPI_RackDeviceCodeData);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 門禁
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_Door : WebAPI_RealtimeData
    {
        public string value => statusTag?.value;
        /// <summary>
        /// 即時狀態Tag
        /// </summary>
        private Tag statusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":Status"));
    }

    /// <summary>
    /// WebAPI即時資料 - 機櫃DeviceCode
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_RackDeviceCode : WebAPI_RealtimeData
    {
        [SerializeField] private WebAPI_RealtimeData_RackDoor frontDoorData, backDoorData;

        public void FindRackDoorData(List<WebAPI_RealtimeData_RackDoor> rackDoorDataList)
        {
            //根據機櫃deviceCode，向RackDoorMap取得對應的RackDoorDeviceCodeInfo，以取得前門與後門的deviceCode，再從WebAPI_RackDoorData中找出對應的資料
            RackDoorDeviceCodeInfo rackDoorDeviceCodeInfo = RackDoorMap.DeviceCodeMap.FirstOrDefault(info => info.Key == deviceCode).Value;
            string frontDoorDeviceCode = rackDoorDeviceCodeInfo.deviceCode_FrontDoor;
            string backDoorDeviceCode = rackDoorDeviceCodeInfo.deviceCode_BackDoor;

            frontDoorData = rackDoorDataList.FirstOrDefault(data => data.deviceCode == frontDoorDeviceCode);
            backDoorData = rackDoorDataList.FirstOrDefault(data => data.deviceCode == backDoorDeviceCode);
        }
    }

    /// <summary>
    /// WebAPI即時資料 - 機櫃門禁
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_RackDoor : WebAPI_RealtimeData
    {
        public bool isConnect => connectStatusTag?.value == "連線";
        public bool isDoorLock => LockStatusTag?.value == "上鎖";
        public bool isDoorOpen => doorStatusTag?.value == "開門";

        /// <summary>
        /// 連線狀態
        /// </summary>
        private Tag connectStatusTag => tags?.FirstOrDefault(tag => tag.tagId.ContainKeyword(":CSA"));
        /// <summary>
        /// 解鎖狀態
        /// </summary>
        private Tag LockStatusTag => tags?.FirstOrDefault(tag => tag.displayName.ContainKeyword("解鎖狀態"));
        /// <summary>
        /// 磁簧開關狀態
        /// </summary>
        private Tag doorStatusTag => tags?.FirstOrDefault(tag => tag.displayName.ContainKeyword("磁簧開關狀態"));
    }
}
