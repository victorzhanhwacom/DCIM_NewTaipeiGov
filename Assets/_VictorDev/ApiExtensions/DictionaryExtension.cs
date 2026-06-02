using System.Collections.Generic;
using System.Linq;
using VzDev.DebugUtils;

namespace VzDev.ApiExtensions
{
    /// 原API類別功能擴充
    public static class DictionaryExtension
    {
        /// [Extended] - 將KeyCollection複製一份List
        public static List<T> CloneKeysAsList<T, V>(this Dictionary<T, V> dictionary) => dictionary.Keys.ToList();

        /// [Extended] - 將ValueCollection複製一份List (V)
        public static List<V> CloneValuesAsList<T, V>(this Dictionary<T, V> dictionary) => dictionary.Values.ToList();
        
        /// [Extended] - 轉成KeyValueData List(Inspector視覺化)
        public static List<KeyValueData<TKey, TValue>> ToKeyValueDataList<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) 
            => dictionary.Select(kvp => new KeyValueData<TKey, TValue>(kvp)).ToList();
    }
}