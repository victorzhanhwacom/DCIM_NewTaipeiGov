using _VictorDev.ApiExtensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _VictorDev.DebugUtils
{
    /// 物件選取外框
    public class ObjectSelectionController : MonoBehaviour
    {
        #region Variables

        [Foldout("[Event] - 選取狀態IsTrue時")] public UnityEvent<bool> onIsSelectedObjectEvent;
        [Foldout("[Event] - 當選取物件時")] public UnityEvent<Transform> onSelectObjectEvent;
        [Foldout("[Event] - 當取消選取時")] public UnityEvent unSelectObjectEvent;
        [Foldout("[組件]"), SerializeField] private Transform selectionBorder;
        [Foldout("[組件]"), SerializeField] private Transform mouseOverBorder;
        [Foldout("[設定]"), SerializeField] private bool isBorderFollowTarget = true;
        [Foldout("[設定]"), SerializeField] private bool isSingleSelection = true;

        private Transform currentSelectTarget;

        #endregion

        /// 選取物件
        public void SelectObject(Transform target)
        {
            if (isSingleSelection && currentSelectTarget != null && currentSelectTarget == target) DeselectObject();
            else
            {
                currentSelectTarget = target;
                ObjectHelper.SetMatchSizeAndPosition(selectionBorder, target);
                selectionBorder.gameObject.SetActive(true);
                onSelectObjectEvent?.Invoke(target);
                onIsSelectedObjectEvent?.Invoke(true);
            }
        }

        /// MouseOver物件
        public void MouseOverObject(Transform target)
        {
            ObjectHelper.SetMatchSizeAndPosition(mouseOverBorder, target);
            mouseOverBorder.gameObject.SetActive(true);
        }
        public void MouseExitObject() => mouseOverBorder.gameObject.SetActive(false);

        /// 取消選取物件
        public void DeselectObject()
        {
            selectionBorder.gameObject.SetActive(false);
            currentSelectTarget = null;
            unSelectObjectEvent?.Invoke();
            onIsSelectedObjectEvent?.Invoke(false);
        }

        private void Update()
        {
            if (isBorderFollowTarget && currentSelectTarget != null)
            {
                selectionBorder.FollowTarget(currentSelectTarget);
            }
        }

        private void OnValidate()
        {
            selectionBorder ??= transform.GetChild(0);
            selectionBorder?.gameObject.SetActive(false);
        }

        private void Start() => OnValidate();
    }
}