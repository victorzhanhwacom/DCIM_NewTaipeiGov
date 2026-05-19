using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _VictorDev.Framework.UIEventUtils
{
    public class UIEventDispatcher : MonoBehaviour
    {
        #region Variables

        [SerializeField] private string eventName;

        public string EventName => eventName.Trim();

        [Foldout("[組件]"), SerializeField] private Button btn;
        [Foldout("[組件]"), SerializeField] private Toggle toggle;

        #endregion

        [Button]
        private void FindComponents()
        {
            TryGetComponent(out btn);
            TryGetComponent(out toggle);
        }
        private void Reset() => FindComponents();

        #region EventListener
        private void OnEnable()
        {
            btn?.onClick.AddListener(SubscribeEventButton);
            toggle?.onValueChanged.AddListener(SubscribeEventToggle);
        }

        private void OnDisable()
        {
            btn?.onClick.RemoveListener(SubscribeEventButton);
            toggle?.onValueChanged.RemoveListener(SubscribeEventToggle);
        }

        private void SubscribeEventButton() => UIEventManager.SubscribeEvent(eventName);
        private void SubscribeEventToggle(bool isOn) => UIEventManager.SubscribeEvent(eventName, isOn);
        #endregion

        private void OnValidate()
        {
            if(string.IsNullOrEmpty(eventName) == false) eventName = eventName.Trim();
        }
    }
}