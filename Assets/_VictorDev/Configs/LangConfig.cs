using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDev.ApiExtensions;
using Newtonsoft.Json;
using UnityEngine;

namespace _VictorDev.Configs
{
    /// 語系類別
    [Serializable]
    public class LangConfig
    {
        /// 文字柵位名
        [JsonProperty] [field:SerializeField]
        public string KeyName { get; private set; }

        
        /// {語系，翻譯後文字}
        [field:SerializeField]
        public List<KeyValueData<EnumLanguage, string>> LangSet { get; private set; }

        public LangConfig(string keyName) => KeyName = keyName;

        /// 設置語系與內容string
        public void AddLang(EnumLanguage language, string value)
        {
            LangSet ??= new ();
            KeyValueData<EnumLanguage, string> findResult = LangSet.Find(x => x.Key == language);
            if (findResult == null)
            {
                LangSet.Add(new KeyValueData<EnumLanguage, string>(language, value));
            }
            else
            {
                findResult.Value = value;
            }
        }

        /// 依語系取得欄位名稱
        public string GetColumnName(EnumLanguage enumLang = EnumLanguage.zh_TW) 
            => LangSet.FirstOrDefault(x => x.Key == enumLang)?.Value;
    }

    /// 語系選擇
    public enum EnumLanguage
    {
        NonSelected = 0,
        zh_TW = 1,
        zh_CN = 2,
        en_US = 3,
    }
}