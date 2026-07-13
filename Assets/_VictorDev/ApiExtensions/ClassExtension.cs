using System.Collections.Generic;
using System.Reflection;
using Debug = VzDev.ToolUtils.Debug;

namespace VzDev.ApiExtensions
{
    /// 原API類別功能擴充
    public static class ClassExtension
    {
        /// [Extended] - 將此類別的變數與其值，輸出成Dictionary
        public static Dictionary<string, string> ToDictionary<T>(this T target) where T : class
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            // 取得所有 public instance 欄位
            FieldInfo[] fields =
                typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    string name = field.Name;
                    string value = field.GetValue(target) as string;
                    result[name] = value;
                }
            }

            return result;
        }

        /// 檢查是否為空值
        /// <para>+ variableName: 可以nameof(變數)取得名稱</para>
        public static bool IsNullOrEmptyWithLog<T>(this T target, string variableName) where T : class
        {
            bool isNull = target == null;
            if (isNull)
                Debug.LogError($"[{variableName.Trim()}] is null.");
            return isNull;
        }
    }
}