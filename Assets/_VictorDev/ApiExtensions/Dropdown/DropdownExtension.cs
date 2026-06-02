using System.Collections.Generic;
using TMPro;

namespace VzDev.ApiExtensions
{
    public static class DropdownExtension
    {
        /// [Extended] -  目前選擇的項目文字
        public static string CurrentSelectedText(this TMP_Dropdown self) => self.options[self.value].text.Trim();

        /// [Extended] -  設定選項清單
        public static void SetOptions(this TMP_Dropdown self, params string[] labels)
        {
            var options = new List<TMP_Dropdown.OptionData>();
            for (var i = 0; i < labels.Length; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(labels[i]));
            }
            self.ClearOptions();      // 清掉舊的
            self.AddOptions(options); // 加入新的
            self.value = 0;
        }
    }
}