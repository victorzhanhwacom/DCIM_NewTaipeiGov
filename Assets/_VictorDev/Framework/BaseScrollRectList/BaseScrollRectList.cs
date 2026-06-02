using System.Collections.Generic;
using System.Data;
using VzDev.ApiExtensions;
using VzDev.DebugUtils;
using VzDev.InterfaceUtils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VzDev.ObjectUtils.ScrollRectUtils
{
    /// [框架：ScrollRect列表] ScrollList
    /// <para> + TData: 資料列表，免帶入List </para>
    public abstract class BaseScrollRectList<TData> : MonoBehaviour, IReceiveData<List<TData>>
    {
        #region Variables

        [Foldout("[Event] - SelectedItem")] public UnityEvent<TData> onSelectedItemEvent;
        [Foldout("[Event] - OnTogglesValueChanged")] public UnityEvent<bool> onTogglesValueChangedEvent;
        [Foldout("[Event] - OnTogglesValueChanged")] public UnityEvent invokeTogglesIsOnEvent, invokeTogglesIsOffEvent;
        [Foldout("[Event] - MouseOver")] public UnityEvent<TData> onPointerEnterEvent;
        [Foldout("[Event] - MouseExit")] public UnityEvent onPointerExitEvent;
        [Foldout("[Event] - IsNoDataEvent")] public UnityEvent<bool> isNoDataEvent;
        
        [Foldout("[組件]"), SerializeField] protected BaseScrollRectListItem<TData> listItemPrefab;
        [Foldout("[組件]"), SerializeField] protected ScrollRect scrollRect;
        [Foldout("[組件]"), SerializeField] private ToggleGroup toggleGroup;

        /// Data列表
        [field:SerializeField]
        public List<TData> DataList { get; protected set; } = new ();

        protected List<BaseScrollRectListItem<TData>> ListItems = new();
        
        public BaseScrollRectListItem<TData> CurrentSelectedItem { get; protected set; }
        
        #endregion
        
        /// 設定Data列表
        public virtual void ReceiveData(List<TData> data)
        {
            DataList = data;
            ClearList();
            UpdateUI(scrollRect.content);
        }
        
        protected virtual void UpdateUI(Transform container = null)
        {
            container ??= scrollRect.content;
            DataList.ForEach(data =>
            {
                BaseScrollRectListItem<TData> item = ObjectHelper.Instantiate(listItemPrefab, container);
                item.SetData(data);
                item.SetToggleGroup(toggleGroup);
                item.OnSelectedItemEvent.AddListener((itemData)=>OnSelectedItemEvent(itemData, item));
                item.OnToggleValueChangedEvent.AddListener(OnTogglesValueChangedEvent);
                item.OnPointerEnterEvent.AddListener(OnPointerEnterEvent);
                item.OnPointerExitEvent.AddListener(OnPointerExitEvent);
                ListItems.Add(item);
            });
            scrollRect.verticalNormalizedPosition = 1;
            isNoDataEvent?.Invoke(ListItems.Count == 0);
        }

        public void CancelSelection()
        {
            if (CurrentSelectedItem == null) return;
            CurrentSelectedItem.SetToggleIsOn(false);
            CurrentSelectedItem = null;
        }

        /// 列表排序
        public void OrderByName(bool isDescending = false) 
            => ObjectHelper.SortTargetsByObjectName<BaseScrollRectListItem<TData>>(scrollRect.content, isDescending);

        #region Event Listener

        private void OnSelectedItemEvent(TData data, BaseScrollRectListItem<TData> item)
        {
            CurrentSelectedItem = item;
            onSelectedItemEvent?.Invoke(data);
        }

        private void OnTogglesValueChangedEvent(bool isOn)
        {
            bool isHaveToggleOn = toggleGroup.AnyTogglesOn();
            (isHaveToggleOn? invokeTogglesIsOnEvent: invokeTogglesIsOffEvent)?.Invoke();
            onTogglesValueChangedEvent?.Invoke(isHaveToggleOn);
        }

        private void OnPointerEnterEvent(TData data) => onPointerEnterEvent?.Invoke(data);
        private void OnPointerExitEvent() => onPointerExitEvent?.Invoke();

        #endregion

        /// 清空列表 
        public void ClearList()
        {
            ListItems = ListItems.ClearMissingTargets();
            ListItems.ForEach(listItems =>
            {
                listItems.OnSelectedItemEvent.RemoveAllListeners();
                listItems.OnToggleValueChangedEvent.RemoveAllListeners();
                listItems.OnPointerEnterEvent.RemoveAllListeners();
                listItems.OnPointerExitEvent.RemoveAllListeners();
                ObjectHelper.Destroy(listItems.gameObject);
            });
            ListItems.Clear();
            scrollRect.content.RemoveAllChildren();
            scrollRect.verticalNormalizedPosition = 1;
            isNoDataEvent?.Invoke(ListItems.Count == 0);
        }


        protected virtual void OnEnable()
        {
            scrollRect.verticalNormalizedPosition = 1;
        }

        protected virtual void OnValidate()
        {
            if(scrollRect == null) scrollRect = GetComponentInChildren<ScrollRect>(true);
            if(toggleGroup == null) toggleGroup = GetComponent<ToggleGroup>();
            ListItems = ListItems.ClearMissingTargets();
        }
    }
}