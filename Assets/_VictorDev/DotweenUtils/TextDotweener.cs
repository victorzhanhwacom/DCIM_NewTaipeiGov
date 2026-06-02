using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace VzDev.DoTweenUtils
{
    /// 文字特效
    public class TextDotweener : MonoBehaviour
    {
        #region Variables
        [Foldout("設定"), SerializeField] private float duration = 0.15f;
        [Foldout("設定"), SerializeField] private float delay = 0.3f;
        [Foldout("設定"), SerializeField] private bool isRandomDelay = true;
        [Foldout("設定"), SerializeField] private TextMeshProUGUI txt;
        #endregion

        public void SetText(string str)
        {
            str = str.Trim();
            if (Application.isPlaying && duration !=0 && delay != 0) DoTweenHelper.ToBlink(txt, str, duration, delay, isRandomDelay);
            else txt.text = str;
        }

        public string text
        {
            set => SetText(value);
        }

        private void Awake() => OnValidate();
        private void OnValidate() => txt ??= GetComponent<TextMeshProUGUI>();
    }
}