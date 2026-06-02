using VzDev.ApiExtensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.TextUtils
{
    /// 檢查string並轉換成超鏈結
    public class TextHyperLinkChecker : MonoBehaviour
    {
        [Foldout("[Event]")] public UnityEvent<string> onTextEvent;
        public void SetText(string text) => onTextEvent?.Invoke(text.ToUrlLinkHtmlTag());
    }
}