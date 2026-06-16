using System;
using System.Collections.Generic;
using System.Linq;
using VzDev.ApiExtensions;
using VzDev.DebugUtils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VzDev.ObjectUtils.UIEventUtils
{
    /// UI組件事件管理器
    /// <para>+ 管理關鍵按鈕所觸發的行為事件</para>
    public class UIEventManager : SingletonMonoBehaviour<UIEventManager>
    {
        #region Variables

        public List<KeyValueData<string, UIEventButtonSet>> uiBtnEvents;
        public List<KeyValueData<string, UIEventToggleSet>> uiToggleEvents;

        private readonly Dictionary<string, UIEventButtonSet> dictionaryBtnEvents =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, UIEventToggleSet> dictionaryToggleEvents =
            new(StringComparer.OrdinalIgnoreCase);

        #endregion

        protected override void Awake()
        {
            base.Awake();
            uiBtnEvents.ForEach(keyValueData =>
            {
                if (!dictionaryBtnEvents.TryAdd(keyValueData.Key, keyValueData.Value))
                    Debug.LogError($"Duplicate button event key: {keyValueData.Key}", this);
            });
            uiToggleEvents.ForEach(keyValueData =>
            {
                if (!dictionaryToggleEvents.TryAdd(keyValueData.Key, keyValueData.Value))
                    Debug.LogError($"Duplicate toggle event key: {keyValueData.Key}", this);
            });
        }

        #region 註冊行為事件Key eventName
        /// 註冊行為事件Key eventName (Button)
        public static void SubscribeEvent(string eventName)
        {
            if (Instance.dictionaryBtnEvents.TryGetValue(eventName, out UIEventButtonSet eventButtonSet))
                eventButtonSet.unityEvent?.Invoke();
            else
                Debug.LogError($"{eventName} is not registered in the UI event manager", Instance);
        }

        /// 註冊行為事件Key eventName (Toggle)
        public static void SubscribeEvent(string eventName, bool isOn)
        {
            if (Instance.dictionaryToggleEvents.TryGetValue(eventName, out UIEventToggleSet eventToggleSet))
                eventToggleSet.unityEvent?.Invoke(isOn);
            else
                Debug.LogError($"{eventName} is not registered in the UI event manager", Instance);
        }
        #endregion

        /// 尋找所有的UIEventDispatchers
        [Button]
        private void FindUIEventDispatchers()
        {
            uiBtnEvents?.Clear();
            uiToggleEvents?.Clear();
            UIEventDispatcher[] result =
                FindObjectsByType<UIEventDispatcher>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .OrderBy(dispatcher => dispatcher.name).ToArray();

            Array.ForEach(result, dispatcher =>
            {
                if (dispatcher.TryGetComponent(out Button _) == false) return;
                if (uiBtnEvents.TyrGetValue(dispatcher.EventName, out UIEventButtonSet eventButtonSet))
                    eventButtonSet.uiEventDispatchers.Add(dispatcher);
                else
                {
                    uiBtnEvents.Add(new KeyValueData<string, UIEventButtonSet>(
                        dispatcher.EventName, new(new() { dispatcher })));
                }
            });

            Array.ForEach(result, dispatcher =>
            {
                if (dispatcher.TryGetComponent(out Toggle _) == false) return;
                if (uiToggleEvents.TyrGetValue(dispatcher.EventName, out UIEventToggleSet eventButtonSet))
                    eventButtonSet.uiEventDispatchers.Add(dispatcher);
                else
                {
                    uiToggleEvents.Add(new KeyValueData<string, UIEventToggleSet>(
                        dispatcher.EventName, new(new() { dispatcher })));
                }
            });
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            uiBtnEvents.ForEach(keyValueData => keyValueData.Key = keyValueData.Key.Trim());
            uiToggleEvents.ForEach(keyValueData => keyValueData.Key = keyValueData.Key.Trim());
        }

        private void Reset() => FindUIEventDispatchers();


        [Serializable]
        public class UIEventButtonSet
        {
            public UnityEvent unityEvent;
            public List<UIEventDispatcher> uiEventDispatchers;

            public UIEventButtonSet(List<UIEventDispatcher> dispatchers) => uiEventDispatchers = dispatchers;
        }

        [Serializable]
        public class UIEventToggleSet
        {
            public UnityEvent<bool> unityEvent;
            public List<UIEventDispatcher> uiEventDispatchers;
            public UIEventToggleSet(List<UIEventDispatcher> dispatchers) => uiEventDispatchers = dispatchers;
        }
    }
}