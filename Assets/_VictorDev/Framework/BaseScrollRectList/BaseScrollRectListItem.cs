using System.Collections.Generic;
using System.Linq;
using VzDev.TextUtils;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VzDev.ObjectUtils.ScrollRectUtils
{
    /// [框架：ScrollRect列表] ScrollList ListItem
    public abstract class BaseScrollRectListItem<TData> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables

        [Foldout("[組件]"), SerializeField] private Toggle toggle;

        [Foldout("[組件]"), SerializeField, Label("\t自動設定Text內容，命名需為Txt+變數名稱")]
        private List<TextMeshProUGUI> txtComps;

        /// [Event] - OnSelectedItem
        public UnityEvent<TData> OnSelectedItemEvent { get; } = new();
        /// [Event] - OnToggleValueChanged
        public UnityEvent<bool> OnToggleValueChangedEvent { get; } = new();

        /// [Event] - MouseEnter
        public UnityEvent<TData> OnPointerEnterEvent { get; } = new();

        /// [Event] - MouseExit
        public UnityEvent OnPointerExitEvent { get; } = new();

        [field:SerializeField]
        public TData Data { get; private set; }
        public bool IsOn => toggle.isOn;
        public bool IsPointerOver { get; private set; }

        #endregion

        /// 設定Toggle.isOn
        public void SetToggleIsOn(bool isOn) => toggle.isOn = isOn;
        
        /// 設定ToggleGroup
        public void SetToggleGroup(ToggleGroup toggleGroup) => toggle.group = toggleGroup;

        /// 設定Data
        public void SetData(TData data)
        {
            Data = data;
            UpdateUI();
        }

        protected virtual void UpdateUI() => TextHelper.SetParamsToTxtComps(Data, txtComps);

        protected virtual void Reset()
        {
            toggle = GetComponent<Toggle>();
            txtComps = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        }

        #region Initialized EventListener

        private void OnEnable() => toggle.onValueChanged.AddListener(OnValueChangedHandler);
        private void OnDisable() => toggle.onValueChanged.RemoveListener(OnValueChangedHandler);

        private void OnValueChangedHandler(bool isOn)
        {
            if (isOn) OnSelectedItemEvent?.Invoke(Data);
            OnToggleValueChangedEvent?.Invoke(isOn);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerOver = true;
            OnPointerEnterEvent?.Invoke(Data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerOver = false;
            OnPointerExitEvent?.Invoke();
        }

        #endregion
    }
}