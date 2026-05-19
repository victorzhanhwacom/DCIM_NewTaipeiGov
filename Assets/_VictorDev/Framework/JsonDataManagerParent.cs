using System.Collections.Generic;
using System.Linq;
using _VictorDev.DebugUtils;
using _VictorDev.InterfaceUtils;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using _VictorDev.ApiExtensions;

namespace _VictorDev.DebugUtils
{
    /// JSON解析資料接收器
    /// <para>+ 僅接收解析後的JSON資料</para>
    /// <para>+ 再透過Event方式傳遞出去，以及傳給每一個註冊的Receiver</para>
    public abstract class JsonDataManagerParent<TData> : MonoBehaviour
    {
        #region Variables

        [Label("[資料項]"), SerializeField] private TData data;
        [Label("[接收器]"), SerializeField] private List<MonoBehaviour> receivers;
        [Foldout("[Event] 是否在讀取資料")] public UnityEvent<bool> isLoadingEvent;
        private List<IReceiveData<TData>> ReceiverReceivers
            => receiverTargets ??= receivers.Cast<IReceiveData<TData>>().ToList();
        private List<IReceiveData<TData>> receiverTargets;

        public TData Data => data;
        
        #endregion

        protected void SetData(TData value) => data = value;
        
        /// 接收JSON資料
        public void ReceiveJson(string jsonString)
        {
            data = JsonConvert.DeserializeObject<TData>(jsonString);
            BeforeInvokeData();
            isLoadingEvent?.Invoke(false);
            InvokeData();
        }
        
        protected virtual void BeforeInvokeData()
        {
        }
        
        /// 發送資料
        public void InvokeData() => ReceiverReceivers.ForEach(receive => receivers.ReceiveData(data));
        protected virtual void OnValidate() => receivers = ObjectHelper.CheckTypeOfList<IReceiveData<TData>>(receivers);
    }
}