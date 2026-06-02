using System;
using UnityEngine;

namespace VzDev.DebugUtils
{
    /// Log訊息處理 (僅在Editor環境下Log)
    public static class Debug
    {
        /// 在Runtime是否要Log訊息
        public static readonly bool IsLogInRuntime = false;

        public static void Log(object message, object callerClass = null, EmojiEnum emojiEnum = EmojiEnum.None,
            bool isPrintArrow = true) =>
            LogMessage(EnumLogType.Log, message, callerClass, emojiEnum, isPrintArrow);

        public static void LogWarning(object message, object callerClass = null, EmojiEnum emojiEnum = EmojiEnum.None,
            bool isPrintArrow = true) =>
            LogMessage(EnumLogType.LogWarning, message, callerClass, emojiEnum, isPrintArrow);

        public static void LogError(object message, object callerClass = null, EmojiEnum emojiEnum = EmojiEnum.None,
            bool isPrintArrow = true) =>
            LogMessage(EnumLogType.LogError, message, callerClass, emojiEnum, isPrintArrow);

        public static bool LogCheckIsNull(object target, object callerClass = null,
            EmojiEnum emojiEnum = EmojiEnum.None)
        {
            bool isNull = target == null;
            if (isNull) LogError($"[{nameof(target)}] is null.", callerClass, emojiEnum);
            return isNull;
        }

        #region Log訊息處理

        /// Log訊息處理
        /// <para> + callerClass使用： GetType().Name / nameOf(T) </para> 
        private static void LogMessage(EnumLogType logType, object message, object callerClass, EmojiEnum emojiEnum,
            bool isPrintArrow)
        {
            if (isLogEnable == false) return;
            //string msg = $"{EmojiHelper.GetEmoji(emojiEnum)} ";
            string msg ="";
            msg += callerClass != null ? $"[ {callerClass} ] " : "";
            msg += (isPrintArrow ? ":> " : " ") + message;
            Action action = null;
            string colorCode;
            switch (logType)
            {
                case EnumLogType.Log:
                    colorCode = "#b7ffbf";
                    //msg = $"<color='{colorCode}'>{msg}</color>";
                    action = () => UnityEngine.Debug.Log(msg);
                    break;
                case EnumLogType.LogWarning:
                    colorCode = "#ffad00";
                    //msg = $"<color='{colorCode}'>{msg}</color>";
                    action = () => UnityEngine.Debug.LogWarning(msg);
                    break;
                case EnumLogType.LogError:
                    colorCode = "#ff9c9c";
                    //msg = $"<color='{colorCode}'>{msg}</color>";
                    action = () => UnityEngine.Debug.LogError(msg);
                    break;
            }

            action?.Invoke();
        }

        /// 檢查是否為Editor環境，是才會Log訊息
        private static bool isLogEnable => (Application.isEditor || IsLogInRuntime);

        #endregion

        /// 判斷condition為false時，才LogWarning message
        public static void Assert(bool condition, string message, object callerClass = null, EmojiEnum emojiEnum = EmojiEnum.Warning)
        {
            if (!condition) LogWarning(message, callerClass, emojiEnum);
        }

        private enum EnumLogType
        {
            Log,
            LogWarning,
            LogError
        }

        // 🎯 Debug 可視化射線（只在 Scene 視窗中可見，不會出現在 Game 視窗）
        public static void DrawRay(Ray ray, float distance, Color? rayColor = null, float duration = 0.01f) =>
            UnityEngine.Debug.DrawRay(ray.origin, ray.direction * distance, rayColor ?? Color.green, duration);
    }
}