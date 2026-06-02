using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VzDev.EventUtils
{
    /// 偵測按鍵以觸發事件
    public class KeyEventMediator : MonoBehaviour
    {
        #region Variables

        [Foldout("[Event]")] public UnityEvent onKeyEvent;
        [Foldout("[設定]"), Label("按下鍵"), SerializeField] private KeyCode keyCode = KeyCode.Tab;
        [Foldout("[設定]"), Label("選擇對像(可Null)"), SerializeField] private GameObject selectedTarget;

        #endregion

        private void Update()
        {
            if (Input.anyKeyDown == false) return;

            if (Input.GetKeyDown(keyCode))
            {
                bool isSelected = selectedTarget == null 
                                  || EventSystem.current?.currentSelectedGameObject == selectedTarget;
                if (isSelected) onKeyEvent?.Invoke();
            }
        }
    }
}