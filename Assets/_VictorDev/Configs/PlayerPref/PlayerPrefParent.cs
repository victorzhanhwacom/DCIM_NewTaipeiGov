using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _VictorDev.Configs
{
    /// PlayerPref資料存儲處理
    public abstract class PlayerPrefParent<T> : MonoBehaviour
    {
        [Foldout("[Event]")] public UnityEvent<T> onValueLoaded;
        [Foldout("[設定]"), SerializeField] private string tagName;
        [Foldout("[設定]"), SerializeField] private T defaultValue;
        [Foldout("[設定]"), SerializeField] private bool isLoadInStart = true;

        protected string TagName
        {
            get
            {
                tagName = tagName.Trim();
                return tagName;
            }
        }
        /// 預設值
        protected T DefaultValue => defaultValue;


        private void Start()
        {
            if(isLoadInStart) LoadValue();
        }

        /// 讀取值
        public abstract void LoadValue(bool notify = true);
        /// 設定TagName
        public void SetTagName(string value) => tagName = value.Trim();
        /// 接收值
        public abstract void Receive(T value);
    }
}