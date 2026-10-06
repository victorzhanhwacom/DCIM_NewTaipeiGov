using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VzDev.NetUtils.WebAPI;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// WebAPI 資料呼叫 - 門禁
    /// </summary>
    public class WebAPI_CallerBase_RealtimeDataDoor : WebAPI_CallerBase<WebAPI_RealtimeData>
    {
        #region Static Event
        public static Action<List<WebAPI_RealtimeData_Door>> OnGetDoorDataAction;
        public static Action<List<WebAPI_RealtimeData_RackDoor>> OnGetRackDoorDataAction;
        #endregion
        #region Field
        [field: SerializeField] public static List<WebAPI_RealtimeData_Door> WebAPI_DoorData { get; private set; }
        [field: SerializeField] public static List<WebAPI_RealtimeData_RackDoor> WebAPI_RackDoorData { get; private set; }
        #endregion

        public override void ParseJson(string json)
        {
            base.ParseJson(json);
            WebAPI_DoorData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("AC"))
                .Select(data => data.ToDoor()).ToList();
            WebAPI_RackDoorData = webapiData?.Where(data => data.deviceCategory.ContainKeyword("Rack"))
                .Select(data => data.ToRackDoor()).ToList();
        }
        public override void InvokeData()
        {
            base.InvokeData();
            OnGetDoorDataAction?.Invoke(WebAPI_DoorData);
            OnGetRackDoorDataAction?.Invoke(WebAPI_RackDoorData);
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
    /// WebAPI即時資料 - 機櫃門禁
    /// </summary>
    [Serializable]
    public class WebAPI_RealtimeData_RackDoor : WebAPI_RealtimeData
    {
    }
}
