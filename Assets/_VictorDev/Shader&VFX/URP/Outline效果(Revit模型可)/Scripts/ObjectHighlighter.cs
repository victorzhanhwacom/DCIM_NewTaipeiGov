using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VzDev.ShaderUtils
{
    public class ObjectHighlighter : MonoBehaviour
    {
        [Foldout("[Event] - 取消選取物件")] [SerializeField]
        private UnityEvent unSelectEvent;

        [Foldout("[Event] - Invoke選取物件")] [SerializeField]
        private UnityEvent<GameObject> onSelectObject;

        [Foldout("[設定]")] [SerializeField] private Toggle toggleLock;
        [Foldout("[設定]")] [SerializeField] private string outlineLayerName = "Outline";
        [Foldout("[設定]")] [SerializeField] private List<string> objectKeywords = new() { "DCR", "DCS", "DCN" };

        private int _outlineLayer;
        private GameObject _lastSelectedObject;
        private int _originalLayer;

        private void Start() => _outlineLayer = LayerMask.NameToLayer(outlineLayerName);

        public void ForceHightlightObject(GameObject target)
            => HighlightHandler(target, true);

        public void HighlightObject(GameObject target)
            => HighlightHandler(target);

        /// Highlight物目標物件
        private void HighlightHandler(GameObject target, bool isForced = false)
        {
            if (toggleLock.isOn == false && isForced == false) return;

            if (objectKeywords.Any(word =>
                    target.name.Contains(word, StringComparison.CurrentCultureIgnoreCase)) == false)
            {
                ClearHighlight();
                return;
            }

            // 當上一個點擊物件不為Null時
            if (_lastSelectedObject != null)
            {
                // 若與上一個點擊物件相同時，則進行取消
                if (_lastSelectedObject == target)
                {
                    ClearHighlight();
                    unSelectEvent?.Invoke();
                    if (isForced == false) return;
                }

                ClearHighlight();
            }

            // 記錄目標物件原本的的Layer
            _lastSelectedObject = target;
            _originalLayer = _lastSelectedObject.layer;
            _lastSelectedObject.layer = _outlineLayer;
            onSelectObject?.Invoke(_lastSelectedObject);
        }

        /// 還原先前的Layer
        public void ClearHighlight()
        {
            if (_lastSelectedObject != null)
            {
                _lastSelectedObject.layer = _originalLayer;
                _lastSelectedObject = null;
            }

            unSelectEvent?.Invoke();
        }
    }
}