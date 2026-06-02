using System;
using System.Collections.Generic;
using System.Linq;
using VzDev.ApiExtensions;
using VzDev.Configs;
using UnityEngine;

namespace VzDev.DebugUtils
{
    public static class EnumHelper
    {
        /// 設定物件對齊軸心座標
        public static float GetAxisAlignPosition<T>(float boundMinAxis, float boundCenterAxis, float boundMaxAxis, T option) where T : Enum =>
            option switch
            {
                EnumAlignmentPivotX.Left => boundMinAxis,
                EnumAlignmentPivotX.Center => boundCenterAxis,
                EnumAlignmentPivotX.Right => boundMaxAxis,
                EnumAlignmentPivotY.Top => boundMaxAxis,
                EnumAlignmentPivotY.Center => boundCenterAxis,
                EnumAlignmentPivotY.Bottom => boundMinAxis,
                EnumAlignmentPivotZ.Front => boundMinAxis,
                EnumAlignmentPivotZ.Center => boundCenterAxis,
                EnumAlignmentPivotZ.Back => boundMaxAxis,
                _ => boundCenterAxis
            };

        /// 設定物件對齊軸心座標
        public static Vector3 GetAxisAlignPosition(Bounds meshBounds, EnumAlignmentPivotX enumAxisX, EnumAlignmentPivotY enumAxisY, EnumAlignmentPivotZ enumAxisZ)
        {
            return new Vector3(
                GetAxisAlignPosition(meshBounds.min.x, meshBounds.center.x, meshBounds.max.x, enumAxisX)
                , GetAxisAlignPosition(meshBounds.min.y, meshBounds.center.y, meshBounds.max.y, enumAxisY)
                , GetAxisAlignPosition(meshBounds.min.z, meshBounds.center.z, meshBounds.max.z, enumAxisZ)
                );
        }
        
        /// 取得指定Enum的內容，集成一個List字串
        public static List<string> EnumToList<T>() where T : struct, Enum => Enum.GetNames(typeof(T)).ToList();
        
        /// 依index取得Enum值
        public static TEnum GetEnumByIndex<TEnum>(int index, TEnum defaultValue = default) where TEnum : Enum
        {
            Array values = Enum.GetValues(typeof(TEnum));
            if (index >= 0 && index < values.Length)
            {
                return (TEnum)values.GetValue(index);
            }
            else
            {
                Debug.LogWarning($"Index 超出 {typeof(TEnum).Name} 範圍！");
                return defaultValue;
            }
        }
        
        /// 依string關鍵字取得Enum值
        public static TEnum GetEnumByString<TEnum>(string keyword, TEnum defaultValue = default) where TEnum : Enum
        {
            keyword = keyword.Trim();
            foreach (TEnum option in Enum.GetValues(typeof(TEnum)))
            {
                if (keyword.ContainKeyword(StringComparison.OrdinalIgnoreCase, option.ToString()))
                    return option;
            }
            return defaultValue;
        }
    }
}