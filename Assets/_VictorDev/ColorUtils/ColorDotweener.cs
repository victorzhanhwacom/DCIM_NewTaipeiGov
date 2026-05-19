using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _VictorDev.ColorUtils
{
    /// 對Graphic組件 (Image/TextmeshProUGUI) 進行DOColor
    public class ColorDotweener : MonoBehaviour
    {
        #region Variables
        [Label("Graphic組件(Image/Text)"), SerializeField] private List<Graphic> targetGraphics = new ();
        [Label("設定by百分比"), SerializeField] private bool isSetByPercentage01 = true;
       
        [Foldout("[設定]"), SerializeField] private float duration = 0.5f;
        [Foldout("[設定]"), SerializeField] private Ease ease = Ease.OutQuad;
        [Foldout("[設定by百分比]"), ReadOnly, SerializeField, ShowIf(nameof(isSetByPercentage01))] private float percentage01;
        [Foldout("[設定by百分比]"), SerializeField, ShowIf(nameof(isSetByPercentage01))] private List<ColorSet> colorSet = new ()
        {
            new ColorSet(0.4f, ColorHelper.HexToColor(0x7BFF69)),
            new ColorSet(0.6f, ColorHelper.HexToColor(0xFFF532)),
            new ColorSet(0.8f, ColorHelper.HexToColor(0xEF701A)),
            new ColorSet(1f, ColorHelper.HexToColor(0x640000)),
        };
        [Foldout("[設定by百分比]"), SerializeField, ShowIf(nameof(isSetByPercentage01))] private float maxValue = 100;
        
        private bool IsSetByBool => !isSetByPercentage01;
        
        [Foldout("[設定byBool]"), SerializeField, ShowIf(nameof(IsSetByBool))] 
        private Color colorOn = Color.white, colorOff = Color.gray;

        private Color _toColor;
        #endregion

        /// 設定By值，轉成百分比
        public void SetByValue(float value) => SetByPercentage01(value / maxValue);
        
        /// 設定By百分比
        public void SetByPercentage01(float percentage)
        {
            percentage01 = percentage;
            _toColor =  colorSet.FirstOrDefault(target=> percentage01 <= target.threshold).color;
            UpdateUI();
        }
        public void SetByBool(bool isOn)
        {
            _toColor = isOn? colorOn : colorOff;
            UpdateUI();
        }

        private void UpdateUI() => targetGraphics.ForEach(target => target.DOColor(_toColor, duration).SetEase(ease));
    }
}