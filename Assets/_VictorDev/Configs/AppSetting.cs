using NaughtyAttributes;
using UnityEngine;

namespace VzDev.Common
{
    /// App設定項目
    public class AppSetting : MonoBehaviour
    {
        [Foldout("[系統設定]"), Label("是否在背景執行"), SerializeField] private bool isRunInBackground = true;
        [Foldout("[系統設定]"), Label("最大FPS限制"), SerializeField, Min(-1)] private int maxFPS = 80;

        private void Start()
        {
            Application.runInBackground = isRunInBackground;
            Application.targetFrameRate = maxFPS;
            QualitySettings.vSyncCount = 0;  // 確保不會被垂直同步 (VSync) 蓋過
        }

        /// 開關Shader
        public void SetShadowIsOn(bool isOn) => QualitySettings.shadows = isOn? ShadowQuality.All : ShadowQuality.Disable;
    }
}