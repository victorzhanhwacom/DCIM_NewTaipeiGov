using UnityEngine;

namespace VzDev.DebugUtils
{
    public static class LayerMaskHelper
    {
        /// 以LayerIndex取得LayerMask名稱
        public static LayerMask IndexToLayerMask(int layerIndex) => 1 << layerIndex;
        public static LayerMask IndexToLayerMask(Transform target) => IndexToLayerMask(target.gameObject.layer);
        
        /// 以LayerMask名稱取得LayerIndex
        public static int LayerMaskToIndex(LayerMask targetLayerMask) => Mathf.RoundToInt(Mathf.Log(targetLayerMask.value, 2));
    }
}