using System;
using System.Collections.Generic;
using VzDev.DebugUtils;
using UnityEngine;

namespace VzDev.TCIT.DCIM
{
    /// DCIM相關處理
    ///                         0       1    2    3   4   5          6
    /// <para> +deviceId範例：NCHC+TAINAN+IDCCO+02F+211+DCR+Modem Rack-MDA19: Modem Rack-MDA19+1 </para>
    public static class DcimHelper
    {
        /// 機櫃單一RackUnit模型尺吋
        public static Vector3 RackUnitSize => new (0.4826f, 0.0445f, 0.9f);

        #region 模型deviceId相關處理

        /// 從模型名稱 取得deviceId
        public static string GetDeviceId(string modelName)
        {
            modelName = modelName.Trim();
            int start = modelName.IndexOf('[');
            int end = modelName.LastIndexOf(']');

            if (start >= 0 && end > start)
            {
                return modelName.Substring(start + 1, end - start - 1);
            }

            return modelName;
        }

        /// 從deviceId 取得專案名稱
        public static string GetProjectName(string deviceId) => deviceId.Split('+')[0];

        /// 從deviceId 取得專案地點
        public static string GetProjectLocation(string deviceId) => deviceId.Split('+')[1];

        /// 從deviceId 取得樓層
        public static string GetRoomFloor(string deviceId) => deviceId.Split('+')[3];

        /// 從deviceId 取得機房代號
        public static string GetRoomCode(string deviceId) => deviceId.Split('+')[4];

        /// 從deviceId 取得設備類型 (DCR、DCS、DCN)
        public static EnumDeviceType GetDeviceType(string deviceId)
            => EnumHelper.GetEnumByString<EnumDeviceType>(deviceId);

        /// 從deviceId 取得設備名稱 (是否包含流水號)
        public static string GetDeviceName(string deviceId, bool isIncludeCode = false)
        {
            if(deviceId.Contains(":") == false)return deviceId;
            string deviceNameAndCode = deviceId.Split(":")[1].Trim();
            if(isIncludeCode) return deviceNameAndCode;
            return deviceNameAndCode.Split("+")[0];
        }

        /// 從deviceId 取得設備類型 (Rack、Server、Router、Switch)
        public static EnumRevitAssetKind GetDeviceKind(string deviceId)
            => EnumHelper.GetEnumByString<EnumRevitAssetKind>(deviceId);
        
        /// 從deviceId 取得設備類型 中文
        public static string GetDeviceKindZh(string deviceId)
            => GetDeviceKindZh(EnumHelper.GetEnumByString<EnumRevitAssetKind>(deviceId));
        /// 從deviceId 取得設備類型 中文
        public static string GetDeviceKindZh(EnumRevitAssetKind revitAssetKind)
            => revitAssetKind switch
            {
                EnumRevitAssetKind.Rack => "機櫃",
                EnumRevitAssetKind.Server => "伺服主機",
                EnumRevitAssetKind.Router => "路由器",
                EnumRevitAssetKind.Switch => "交換機",
                EnumRevitAssetKind.ODF => "光纖配線架",
                EnumRevitAssetKind.DF => "網路配線架",
                EnumRevitAssetKind.RackStation => "NAS網路儲存伺服器",
                _=> "未知"
            };
        #endregion

       
    }

    #region Enum

    [Serializable]
    public enum EnumDeviceType
    {
        DCR,
        DCN,
        DCS
    }

    [Serializable]
    public enum EnumRevitAssetKind
    {
        Unknown,
        
        /// 機櫃
        Rack,
        /// 伺服主機
        Server,
        /// 路由器
        Router,
        /// 交換器
        Switch,
        /// 光纖配線架
        ODF,
        /// 網路配線架
        DF,
        /// NAS
        RackStation,
    }

    #endregion
}