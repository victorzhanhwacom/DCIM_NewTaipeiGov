using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _VictorDev.DoTweenUtils
{
    /// 對Graphic組件 (Image/TextmeshProUGUI) 進行DOColor
    public class ColorDotweener : _DotweenerBase
    {
        public void SetEnabled(bool isEnabled) =>
            targets.ForEach(target =>
                SetDelayAndEase(target.DOColor(isEnabled ? colorEnabled : colorDisabled, Duration)));

        [Foldout("設定Enabled/Disabled")] [SerializeField]
        private Color colorEnabled = Color.white, colorDisabled = Color.white * 0.2f;

        [Label("對像Image/TextmeshProUGUI")] [SerializeField]
        private List<Graphic> targets;
    }
}