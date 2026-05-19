using System.Collections.Generic;
using _VictorDev.InterfaceUtils;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using _VictorDev.Configs;

namespace _VictorDev.ScrollRectUtils.OLD
{
    /// 樣版：ScrollRect
    public abstract class BaseScrollRectList<TData, TItem> : MonoBehaviour, IReceiveData<List<TData>>
        where TData : class where TItem : BaseListItemPrefab<TData>
    {
        [Foldout("Event"), SerializeField] private UnityEvent<TItem> onItemSelected;
        [Foldout("[組件]"), SerializeField] private ScrollRect scrollRect;
        [Foldout("[組件]"), SerializeField] private TItem itemPrefab;
        [Foldout("[組件]"), SerializeField] private ToggleGroup toggleGroup;
        [Foldout("[組件]"), SerializeField] private TextMeshProUGUI txtAmount;

        private List<TData> _dataList;
        private TItem _selectedItem;

        public void ReceiveData(List<TData> dataList)
        {
            _dataList = dataList;
            ClearList();
            UpdateUI();
        }

        private void UpdateUI()
        {
            _dataList.ForEach(data =>
            {
                TItem item = Instantiate(itemPrefab, scrollRect.content);
                item.ReceiveData(data);
                item.SetToggleGroup(toggleGroup);
                item.onClickEvent.AddListener((data) =>
                {
                    _selectedItem = item;
                    onItemSelected?.Invoke(item);
                });
            });

            if (txtAmount != null) txtAmount.SetText(_dataList.Count.ToString());
        }

        private void ClearList()
        {
            List<GameObject> childrenToDestroy = new List<GameObject>();
            foreach (Transform child in scrollRect.content)
            {
                childrenToDestroy.Add(child.gameObject);
            }

            childrenToDestroy.ForEach(Destroy);
            scrollRect.verticalNormalizedPosition = 1;
        }

        public void CancelSelection() => _selectedItem.SetToggleOn(false);
    }
}