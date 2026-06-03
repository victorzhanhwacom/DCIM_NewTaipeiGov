using UnityEngine;

namespace Heatmap
{
    public class HeatSource : MonoBehaviour
    {
        [Header("熱源參數")]
        /* [Range(0f, 50f)]
    public float temperature = 25f;    */
        [Range(0f, 100f)]
        public float temperature = 1.0f;      // 熱源強度 0~1 

        [Range(0.01f, 1f)]
        public float radius = 0.15f;          // 影響半徑（Gaussian sigma，相對於 Volume 空間）

        [Range(0.5f, 5f)]
        public float falloff = 2.0f;          // 衰減指數（越大越集中）

        public bool isActive = true;

        // 供 Manager 讀取
        public float Temperature => isActive ? temperature : 0f;
        public float Radius => radius;
        public float Falloff => falloff;
    }
}