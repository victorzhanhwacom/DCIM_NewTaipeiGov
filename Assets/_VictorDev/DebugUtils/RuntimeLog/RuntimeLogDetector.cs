using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.ApiExtensions
{
    /// 偵測LOG事件訊息 (Runtime)
    public class RuntimeLogDetector : MonoBehaviour
    {
        #region Variables

        [Foldout("[Event] Log事件訊息")] public UnityEvent<string> onRuntimeLogEvent;

        [Label("偵測訊息類型"), SerializeField] private List<LogType> logTypes = new()
        {
            LogType.Exception, LogType.Error,
        };
        private readonly object _fileLock = new ();

        #endregion

        /// 處理Log訊息
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (logTypes.Contains(type) == false) return;
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string formattedMsg =
                $"[{timestamp}] [{type}]\n{logString}\n========== StackTrace ==========\n{stackTrace}\n";
            onRuntimeLogEvent?.Invoke(formattedMsg);
        }
        private void OnEnable() => Application.logMessageReceivedThreaded += HandleLog;
        private void OnDisable() => Application.logMessageReceivedThreaded -= HandleLog;
    }
}