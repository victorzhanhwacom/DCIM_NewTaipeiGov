using System;
using UnityEngine;

namespace _VictorDev.ColorUtils
{
    [Serializable]
    public struct ColorSet
    {
        [Range(0,1)]
        public float threshold;
        public Color color;

        public ColorSet(float threshold, Color color)
        {
            this.threshold = threshold;
            this.color = color;
        }
    }
}