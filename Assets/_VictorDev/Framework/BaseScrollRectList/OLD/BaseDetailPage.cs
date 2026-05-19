using _VictorDev.InterfaceUtils;
using UnityEngine;
using _VictorDev.Configs;

namespace _VictorDev.ScrollRectUtils
{
    /// 樣版：詳細頁面
    public abstract class BaseDetailPage<TItem, TData> : MonoBehaviour, IReceiveData<TItem>
        where TItem : BaseListItemPrefab<TData>
        where TData : class
    {
        protected TItem Item;
        protected TData Data => Item.Data;

        public void ReceiveData(TItem item)
        {
            Item = item;
            UpdateUI();
        }

        protected abstract void UpdateUI();
    }
}