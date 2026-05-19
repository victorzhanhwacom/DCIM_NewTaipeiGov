using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _VictorDev.ScrollRectUtils
{
    /// 樣版：ScrollRect的ListItem
    public abstract class BaseListItemPrefab<TData> : MonoBehaviour where TData : class
    {
        [Foldout("[Event]")] public UnityEvent<BaseListItemPrefab<TData>> onClickEvent;
        [Foldout("[組件]"), SerializeField] private Toggle toggle;
        [Foldout("[組件]"), SerializeField] private TextMeshProUGUI txtLabel;
        public TData Data { get; private set; }

        public void ReceiveData(TData data)
        {
            Data = data;
            UpdateUI();
        }

        protected abstract void UpdateUI();

        public void SetToggleGroup(ToggleGroup toggleGroup) => toggle.group = toggleGroup;
        public void SetToggleOn(bool isOn) => toggle.isOn = isOn;

        private void OnEnable() => toggle.onValueChanged.AddListener(OnValueChanged);
        private void OnDisable() => toggle.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(bool isOn)
        {
            if (isOn) onClickEvent?.Invoke(this);
        }
    }

    public interface IBaseItemPrefab
    {
        string Label { get; }
    }
}