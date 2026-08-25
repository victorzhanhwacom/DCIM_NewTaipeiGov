using System.Collections.Generic;
using System.Linq;
using VzDev.ApiExtensions;
using VzDev.Configs;
using VzDev.DebugUtils;
using VzDev.FileUtils;
using NaughtyAttributes;
using UnityEngine;
using VzDev.UnityAPI.Extensions;

namespace VzDev.DcimUtils.DCIM
{
    /// DCIM App設定檔
    public class DcimSysConfig : SingletonMonoBehaviour<DcimSysConfig>
    {
        #region Variables

        [Label("[COBie語系設定]"), SerializeField] private List<LangConfig> cobieLangConfig;
        [Foldout("[設定]"), SerializeField] private TextFileLoader csvFileLoader;


        [Foldout("[環控設定]"), SerializeField] private Vector2 rtValueRange, rhValueRange;
        [Foldout("[環控設定]"), SerializeField] private Vector4 rtValueThreshold, rhValueThreshold;
        [Foldout("[環控設定]"), SerializeField] private Vector2 rtValueRangeDEMO, rhValueDEMO;
        [Foldout("[環控設定]"), SerializeField] private Gradient heatColor;
        [Foldout("[環控設定]"), SerializeField] private Gradient humidityColor;
        
        public static Vector2 RTValueValueRange => Instance.rtValueRange;
        public static  Vector2 RhValueValueRange => Instance.rhValueRange;

        public static Vector2 RTValueValueRangeDEMO => Instance.rtValueRangeDEMO;
        public static  Vector2 RhValueValueRangeDEMO => Instance.rhValueDEMO;
        
        #endregion

        /// 取得百分比的顏色
        public static Color GetPercentHeatColor(float percent) => Instance.heatColor.Evaluate(percent);
        public static Color GetPercentHumidityColor(float percent) => Instance.humidityColor.Evaluate(percent);
        
        public static string GetCobieColumnNames(string keyName)
        {
            keyName = keyName.Trim();
            return Instance.cobieLangConfig.FirstOrDefault(set => set.KeyName == keyName)?.GetColumnName();
        }

        [Button]
        private void LoadLangCsvFile() => csvFileLoader.LoadTextFile();

        public void ReceiveLangConfigTextFileData(string textFileData)
        {
            cobieLangConfig?.Clear();
            cobieLangConfig ??= new List<LangConfig>();
            
            // 先把每一行分出來（過濾空行）
            var lines = textFileData.SplitToLines();
            // 跳過標題列
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length >= 3)
                {
                    string keyName = parts[0].Trim();
                    string[] keyNameSplit = parts[0].Trim().Split('_');
                    string langUs = keyNameSplit[0].ToCapitalizeFirstLetter() + keyNameSplit[1].ToCapitalizeFirstLetter(); 
                    string langTw = parts[2].Trim(); 
                    LangConfig langConfig = new LangConfig(keyName);
                    langConfig.AddLang(EnumLanguage.en_US, langUs);
                    langConfig.AddLang(EnumLanguage.zh_TW, langTw);
                    cobieLangConfig.Add(langConfig);
                }
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            csvFileLoader ??= transform.Find("CSVFileLoader").GetComponentInChildren<TextFileLoader>();
        }
    }
}