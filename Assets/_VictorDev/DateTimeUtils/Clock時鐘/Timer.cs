using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VzDev.Configs;
using _VictorDEV.DateTimeUtils;
using VzDev.DebugUtils;
using VzDev.MediatorUtils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.DateTimeUtils
{
    /// 計時器 (Coroutine制)
    public class Timer : MonoBehaviour
    {
        #region Variables

        [Label("是否在Start自動執行"), SerializeField]
        private bool isActiveInStart;

        [Label("是否無窮迴圈"), SerializeField] private bool isInfiniteLoop = true;
        private bool IsNotInfiniteLoop => !isInfiniteLoop;

        [Foldout("[Event] - Timer每次間隔變動時Invoke (DateTime.now)")]
        public UnityEvent<string> onTimeUpdated;


        [Foldout("[設定]"), SerializeField, Label("時間間隔單位")]
        private EnumTime enumTime = EnumTime.秒;

        private bool IsTimeIntervalSec => enumTime == EnumTime.秒;
        private bool IsTimeIntervalMin => enumTime == EnumTime.分;
        private bool IsTimeIntervalHour => enumTime == EnumTime.時;

        [Foldout("[設定]"), SerializeField, Min(0.01f), ShowIf(nameof(IsTimeIntervalSec))]
        private float intervalSecond = 300;

        [Foldout("[設定]"), SerializeField, Min(0.01f), ShowIf(nameof(IsTimeIntervalMin))]
        private float intervalMinute = 5;

        [Foldout("[設定]"), SerializeField, Min(0.01f), ShowIf(nameof(IsTimeIntervalHour))]
        private float intervalHour = 1;

        [Foldout("[設定]"), SerializeField] private EnumTimeFormat invokeTimeFormat = EnumTimeFormat.時分秒_12小時制;
        [Foldout("[設定]"), SerializeField, Min(0f), Label("偏移值(小時)]")] private float offsetHour;

        [Foldout("[設定]"), Label("迴圈次數"), SerializeField, ShowIf(nameof(IsNotInfiniteLoop)), Min(1)]
        private int maxCount = 1;

        [Foldout("[Event] - Timer結束時Invoke (DateTime.now)"), ShowIf(nameof(IsNotInfiniteLoop))]
        public UnityEvent<string> onTimerFinished;


        [Label("[ITimer對像]"), SerializeField] private List<MonoBehaviour> iTimerMonoBehaviour;
        private List<ITimerUpdate> iTimerUpdateTargets;
        private List<ITimerFinish> iTimerFinishTargets;

        public bool IsTimerRunning => timerCoroutine != null;

        /// 目前時間String
        private string GetNowDateTimeString() => DateTime.Now.AddHours(offsetHour)
            .ToString(DateTimeHelper.GetTimeFormat(invokeTimeFormat), new CultureInfo("en-US"));

        private Coroutine timerCoroutine;

        #endregion
        
        private IEnumerator UpdateTimer()
        {
            int loopCounter = 0;

            while (isInfiniteLoop || loopCounter < maxCount)
            {
                int totalIntervalSeconds = DateTimeHelper.CalculatedToTotalSeconds(
                    IsTimeIntervalHour ? intervalHour : 0,
                    IsTimeIntervalMin ? intervalMinute : 0,
                    IsTimeIntervalSec ? intervalSecond : 0
                );
                yield return new WaitForSeconds(totalIntervalSeconds);
                OnTimeUpdated();
                loopCounter++;
            }

            if (!isInfiniteLoop) OnTimerFinished();
        }

        public void OnTimeUpdated()
        {
            onTimeUpdated?.Invoke(GetNowDateTimeString());
            iTimerUpdateTargets?.ForEach(target=> target.OnTimeUpdate());
        }
        
        public void OnTimerFinished()
        {
            onTimerFinished?.Invoke(GetNowDateTimeString());
            iTimerFinishTargets.ForEach(target=> target.OnTimeFinished());
        }

        /// 開始時鐘
        [Button]
        public void StartTimer()
        {
            StopTimer(false);
            if (!IsTimerRunning) timerCoroutine = StartCoroutine(UpdateTimer());
        }

        /// 停止時鐘
        [Button]
        public void StopTimer(bool isInvokeEvent = true)
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            if (isInvokeEvent) OnTimerFinished();
        }

        private void Start()
        {
            if (!Application.isPlaying) return; //防止編輯器場景中重新載入時意外觸發計時器（特別是做 editor tool 時）。
            if (isActiveInStart) StartTimer();
        }

        private void OnValidate() => iTimerMonoBehaviour = ObjectHelper.CheckTypesOfList(iTimerMonoBehaviour
            , BoolLogicGate.BoolGateType.OR, typeof(ITimerUpdate), typeof(ITimerFinish));

        private void Awake()
        {
            iTimerUpdateTargets = iTimerMonoBehaviour.OfType<ITimerUpdate>().ToList();
            iTimerFinishTargets = iTimerMonoBehaviour.OfType<ITimerFinish>().ToList();
        }
    }


    public interface ITimerUpdate
    {
        /// 每讀完一個循環時
        void OnTimeUpdate();
    }
    
    public interface ITimerFinish
    {
        /// 當Timer結束時
        void OnTimeFinished();
    }
}