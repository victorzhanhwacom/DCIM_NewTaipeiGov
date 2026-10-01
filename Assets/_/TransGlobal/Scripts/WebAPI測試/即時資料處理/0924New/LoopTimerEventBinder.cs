using NaughtyAttributes;
using UnityEngine;
using VzDev.DateTimeUtils;
using VzDev.NetUtils.WebAPI;

namespace VzDev.DCIMUtils
{
    /// <summary>
    /// 設置WebAPI呼叫的TimerController事件綁定
    /// <para> + NoResponseCall: 逾時未回應時, 會自動重新呼叫WebAPI </para>
    /// <para> + LoopCall: 每隔一段時間, 會自動呼叫WebAPI </para>
    /// </summary>
    public class LoopTimerEventBinder : MonoBehaviour
    {
        [SerializeField] private WebAPI_CallerBase<WebAPI_RealtimeData> targetCaller;
        [SerializeField] private TimerController noResponseCall, loopCall;

        #region 綁定TimerController事件
        private void BindEvents(bool isBind = true)
        {
            if (isBind)
            {
                // 呼叫時
                targetCaller.onCallingEvent.onStartEvent.AddListener(noResponseCall.StartTimer);
                // 呼叫完成時
                targetCaller.onCallingEvent.callingStatusEvent.AddListener(OnCallingStatusEvent);
                targetCaller.onCallingEvent.callingStatusEvent.AddListener(OnCallingStatusEvent);
                // 停止呼叫時
                targetCaller.onCallingEvent.onStopEvent.AddListener(noResponseCall.StopTimer);
                targetCaller.onCallingEvent.onStopEvent.AddListener(loopCall.StopTimer);
                // 逾時未回應時
                noResponseCall.onTimerEnd.AddListener(targetCaller.RecallWebAPI);
                // 時間間隔時呼叫
                loopCall.onTimerEnd.AddListener(targetCaller.CallWebAPI);
            }
            else
            {
                targetCaller.onCallingEvent.onStartEvent.RemoveListener(noResponseCall.StartTimer);
                targetCaller.onCallingEvent.callingStatusEvent.RemoveListener(OnCallingStatusEvent);
                targetCaller.onCallingEvent.callingStatusEvent.RemoveListener(OnCallingStatusEvent);
                targetCaller.onCallingEvent.onStopEvent.RemoveListener(noResponseCall.StopTimer);
                targetCaller.onCallingEvent.onStopEvent.RemoveListener(loopCall.StopTimer);
                noResponseCall.onTimerEnd.RemoveListener(targetCaller.RecallWebAPI);
                loopCall.onTimerEnd.RemoveListener(targetCaller.CallWebAPI);
            }
        }

        private void OnCallingStatusEvent(bool isCalling)
        {
            if (isCalling == false)
            {
                noResponseCall.StopTimer();
                loopCall.StartTimer();
            }
        }
        #endregion

        private void OnEnable()
        {
            BindEvents(true);
            SystemConfigHandler.OnGetSystemConfigAction += OnGetSystemConfig;
            OnGetSystemConfig(SystemConfigHandler.SystemConfig);
        }
        private void OnDisable()
        {
            BindEvents(false);
            SystemConfigHandler.OnGetSystemConfigAction -= OnGetSystemConfig;
        }

        private void OnGetSystemConfig(SystemConfig config)
        {
            noResponseCall.SetTimeValue(config.webapi.requestTimeoutSec);
            loopCall.SetTimeValue(config.webapi.requestIntervalSec);
        }

        private void Awake() => GetComponents();

        [Button]
        private void GetComponents()
        {
            targetCaller ??= GetComponentInParent<WebAPI_CallerBase<WebAPI_RealtimeData>>();
            noResponseCall ??= transform.Find("NoResponseCall").GetComponent<TimerController>();
            loopCall ??= transform.Find("LoopCall").GetComponent<TimerController>();
        }

        private void OnValidate() => GetComponents();
    }
}
