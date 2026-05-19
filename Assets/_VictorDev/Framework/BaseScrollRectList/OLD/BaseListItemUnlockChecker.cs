using _VictorDev.InterfaceUtils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using _VictorDev.Configs;

namespace _VictorDev.ScrollRectUtils
{
    /// 僅判斷對像物件(IUnLockable)的Unlock狀態，來Invoke相對像的事件
    public abstract class BaseListItemUnlockChecker<TItem, TData>: MonoBehaviour, IReceiveData<TItem>
        where TItem : BaseListItemPrefab<TData>
        where TData : class
    {
        [Foldout("[Event] 點擊Item為已解鎖Unlocked時")]
        public UnityEvent<TItem> onItemClickedUnlockedEvent;
        [Foldout("[Event] 點擊Item為已上鎖Lock時")] 
        public UnityEvent<TItem> onItemClickedLockedEvent;

        public TItem Item { get; private set; }

        public void ReceiveData(TItem item)
        {
            Item = item;
            if (Item is IUnLockable unLockable)
            {
                if(unLockable.IsUnlock) onItemClickedUnlockedEvent?.Invoke(Item);
                else onItemClickedLockedEvent?.Invoke(Item);
            }
        }
    }
}