using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.Advanced
{
    public class FixedModelPivot : MonoBehaviour
    {
        #region Variables

        [Foldout("[組件]"), SerializeField] private MeshRenderer meshRenderer;

        #endregion


        public void SetPosition(Vector3 pos)
        {
            Vector3 modelCenter = meshRenderer.bounds.center;
            // 偏移量 = 中心 - Pivot
            Vector3 offset = modelCenter - transform.position;
            transform.position = pos - offset;
        }

        private void OnValidate()
        {
            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetRotation(Vector3 zero)
        {
            throw new System.NotImplementedException();
        }
    }
}