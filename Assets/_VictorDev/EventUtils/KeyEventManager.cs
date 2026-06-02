using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.EventUtils
{
    /// 偵測按鍵以觸發事件
    public class KeyEventManager : MonoBehaviour
    {
        [Label("[按鍵事件]"), SerializeField] private List<KeyEventSet> keyEventSets;

        private void Update()
        {
            if (keyEventSets.Count != 0 && Input.anyKeyDown)
            {
                foreach (KeyEventSet set in keyEventSets)
                {
                    if (Input.GetKeyDown(set.keyCode))
                    {
                        set.onKeyEvent?.Invoke();
                    }
                }
            }
        }

        [Serializable]
        public class KeyEventSet
        {
            public KeyCode keyCode = KeyCode.None;
            public UnityEvent onKeyEvent;
        }
    }
}