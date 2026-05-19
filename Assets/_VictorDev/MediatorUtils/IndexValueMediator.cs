using System.Collections.Generic;
using _VictorDev.ApiExtensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _VictorDev.DebugUtils
{
    /// Index值處理事件仲介
    public class IndexValueMediator : MonoBehaviour
    {
        #region Variables

        [Label("[Event設定] Invoke是否被選取")] public List<KeyValueData<int, UnityEvent<bool>>> indexBoolEventSetting;
        [Label("[Event設定] Index各別觸發")] public List<KeyValueData<int, UnityEvent>> indexEventSetting;

        #endregion

        /// 設定Index值
        public void SetIndexValue(int indexValue)
        {
            indexBoolEventSetting.ForEach(keyPair =>
            {
                keyPair.Value?.Invoke(keyPair.Key == indexValue);
            });
            
            indexEventSetting.ForEach(keyPair =>
            {
                if(keyPair.Key == indexValue) keyPair.Value?.Invoke();
            });
        }
    }
}