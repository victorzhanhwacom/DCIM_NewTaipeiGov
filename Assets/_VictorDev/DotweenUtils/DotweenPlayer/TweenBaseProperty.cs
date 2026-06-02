using System;
using DG.Tweening;

namespace VzDev.DebugUtils
{
    [Serializable]
    public class TweenBaseProperty
    {
        public float duration = 0.5f, delay;
        public Ease ease = Ease.OutQuad;
    }
}