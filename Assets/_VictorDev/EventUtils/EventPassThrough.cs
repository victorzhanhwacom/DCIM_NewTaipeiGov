using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VzDev.EventUtils
{
    /// 將滑鼠事件傳給上一層UI
    /// <para>+ 需跟互動類型的UI組件放在一起</para>
    /// <para>+ 例：Image / Toggle / Text / Dropdown / EventTrigger</para>
    public class EventPassThrough : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Variables

        [InfoBox("需跟互動類型的UI組件放在一起\n例：Image / Toggle / Text / Dropdown / EventTrigger")]
        [SerializeField, Label("Click/PointerDownUp事件是否傳給上一層UI物件")] private bool isPassClickEventToParent = false;
        [SerializeField, Label("是否額外增加事件控制")] private bool isAdditionEvent;

        [Foldout("\t[Event] - onClick"), ShowIf(nameof(isAdditionEvent))]
        public UnityEvent<PointerEventData> onClick;
        
        [Foldout("\t[Event] - onPointerDown / onPointerUp"), ShowIf(nameof(isAdditionEvent))]
        public UnityEvent<bool> isPointerDown;
        [Foldout("\t[Event] - onPointerDown / onPointerUp"), ShowIf(nameof(isAdditionEvent))]
        public UnityEvent<PointerEventData> onPointerDown, onPointerUp;
        
        [Foldout("\t[Event] - onBeginDrag / onEndDrag"), ShowIf(nameof(isAdditionEvent))]
        public UnityEvent<bool> isDragging;
        [Foldout("\t[Event] - onBeginDrag / onEndDrag"), ShowIf(nameof(isAdditionEvent))]
        public UnityEvent<PointerEventData> onBeginDrag, onEndDrag;

        [Foldout("\t[Event] - onDrag"), ShowIf(nameof(isAdditionEvent))]
        public UnityEvent<PointerEventData> onDrag;
        
        /// 是否正在Dragging
        private bool isActuallyDragging;
        #endregion

        /// 傳遞Event給父物件
        private void PassEventToParent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            // 向上找可處理的父物件（如 ScrollRect）
            Transform parent = transform.parent;
            GameObject current = gameObject;
            
            while (parent != null)
            {
                // 防止循環或自己吃到自己事件
                if (parent.gameObject == current)
                    break;
                
                if (ExecuteEvents.Execute(parent.gameObject, data, function))
                    break;
                parent = parent.parent;
            }
        }
        
        #region Event事件
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(eventData);
            isPointerDown?.Invoke(true);
            if(isPassClickEventToParent)  PassEventToParent(eventData, ExecuteEvents.pointerDownHandler);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke(eventData);
            isPointerDown?.Invoke(false);
            if(isPassClickEventToParent)  PassEventToParent(eventData, ExecuteEvents.pointerUpHandler);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isActuallyDragging) return; // 防止拖完又觸發點擊
            onClick?.Invoke(eventData);
            if(isPassClickEventToParent)  PassEventToParent(eventData, ExecuteEvents.pointerClickHandler);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isActuallyDragging = true;
            isDragging?.Invoke(isActuallyDragging);
            onBeginDrag?.Invoke(eventData);
            PassEventToParent(eventData, ExecuteEvents.beginDragHandler);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
            PassEventToParent(eventData, ExecuteEvents.dragHandler);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isActuallyDragging = false;
            isDragging?.Invoke(isActuallyDragging);
            onEndDrag?.Invoke(eventData);
            PassEventToParent(eventData, ExecuteEvents.endDragHandler);
        }
        #endregion
        
        /// For動態加載組件時初始化
        private void Awake()
        {
            onPointerDown ??= new UnityEvent<PointerEventData>();
            onPointerUp ??= new UnityEvent<PointerEventData>();
            onClick ??= new UnityEvent<PointerEventData>();
            onBeginDrag ??= new UnityEvent<PointerEventData>();
            onDrag ??= new UnityEvent<PointerEventData>();
            onEndDrag ??= new UnityEvent<PointerEventData>();
            isPointerDown ??= new UnityEvent<bool>();
            isDragging ??= new UnityEvent<bool>();
        }
    }
}