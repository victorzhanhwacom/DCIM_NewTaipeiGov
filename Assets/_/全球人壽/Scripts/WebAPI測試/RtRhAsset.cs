using System;
using VzDev.DCIMUtils.DataUtils;

[Serializable]
public class RtRhAsset : DCIMAsset
{
    public Tags rtTag, rhTag;

    [Serializable]
    public struct Tags
    {
        public string title;
        public float value;
        public string localTimestamp;

        public Alert alert;
    }

     [Serializable]
    public struct Alert
    {
        /// <summary>
        /// 目前的警報階層：
        /// 0=正常（目前無未解除警報）；1~98=已觸發警報（數字越大越嚴重）； 99=離線。
        /// 除了對應 Redis 未解除事件暫存外，若 Redis 即時數據（sensor:realtime）完全 查無這筆資料，
        /// 無論告警引擎是否已正式判定，一律強制回傳 99（Message 固定為「離線」）， 
        /// 讓「查無即時數據」能立即反映，不需要等告警引擎的持續時間門檻確認。 
        /// Direction=1（寫入/控制）的點位不做離線判斷、不觸發離線告警，
        /// AlertLevel 固定為 0 （即使即時值查無資料，或未解除事件暫存剛好有殘留告警，也一律視為正常）。
        /// </summary>
        public int alertLevel;
        /// <summary>
        /// 警報訊息
        /// </summary>
        public string message;

        /// <summary>
        /// 警報嚴重度：Critical/High/Medium/Low/Info
        /// </summary>
        public string severity;
    }
}
