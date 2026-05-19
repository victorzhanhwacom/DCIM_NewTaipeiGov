using _VictorDev.ApiExtensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _VictorDev.TextUtils
{
    /// 檢查string並轉換成超鏈結
    public class TextHyperLinkChecker : MonoBehaviour
    {
        [Foldout("[Event]")] public UnityEvent<string> onTextEvent;
        public void SetText(string text) => onTextEvent?.Invoke(text.ToUrlLinkHtmlTag());
    }
}