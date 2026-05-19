using _VictorDev.Framework.ScrollRectUtils;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _VictorDev.UIComps
{
    public abstract class SearchBar<TData> : BaseScrollRectList<TData>, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables

        [Foldout("[Event] - Invoke搜尋字")] public UnityEvent<string> onKeyInputEvent;
        [Foldout("[Event] - Invoke搜尋字")] public UnityEvent<string> onSubmitEvent;
        [Foldout("[組件]"), SerializeField] private TMP_InputField inputField;
        [Foldout("[設定]"), SerializeField, Min(3)] private int minKeywordLength = 6;
        #endregion

       
        public void OnKeyInput(string keyword)
        {
            if (keyword.Length < minKeywordLength) return;
            onKeyInputEvent?.Invoke(keyword.Trim());
            isNoDataEvent?.Invoke(false);
        }
        
        public void Search() => OnSubmit(inputField.text.Trim());
        
        private void OnSubmit(string keyword)
        {
            ClearList();;
            onSubmitEvent?.Invoke(keyword.Trim());
            isNoDataEvent?.Invoke(false);
            OnDeselect(keyword);
        }

        private void OnDeselect(string keyword)
        {
            bool isShowList = string.IsNullOrEmpty(keyword.Trim()) != true || ListItems.Count > 0;
            scrollRect.transform.parent.gameObject.SetActive(isShowList);
        }
        
        
        
        protected override void OnEnable()
        {
            base.OnEnable();
            inputField.onValueChanged.AddListener(OnKeyInput);
            inputField.onSubmit.AddListener(OnSubmit);
            inputField.onDeselect.AddListener(OnDeselect);
        }

        private void OnDisable()
        {
            inputField.onValueChanged.RemoveListener(OnKeyInput);
            inputField.onSubmit.RemoveListener(OnSubmit);
            inputField.onDeselect.RemoveListener(OnDeselect);
        }

        private void Reset()
        {
            inputField = GetComponentInChildren<TMP_InputField>(true);
            OnValidate();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnDeselect(inputField.text);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            scrollRect.transform.parent.gameObject.SetActive(false);
        }
    }
}