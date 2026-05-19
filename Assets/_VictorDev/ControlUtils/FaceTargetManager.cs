using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.ControlUtils
{
    /// 管理浮動物件，面向目標物件
    public class FaceTargetManager : MonoBehaviour
    {
        [SerializeField] private Transform[] objects; // Assign all landmark transforms
        [SerializeField] private Transform targetTransform;
        private Quaternion _targetRotation;
        
        private bool IsHaveObject => objects.Length > 0;

        void Start()
        {
            if (Camera.main != null) targetTransform ??= Camera.main.transform;
        }

        void LateUpdate() => FaceToTarget();

        [Button, ShowIf(nameof(IsHaveObject))]
        private void FaceToTarget()
        {
            _targetRotation = targetTransform.rotation;
            int count = objects.Length;

            for (int i = 0; i < count; i++)
            {
                objects[i].rotation = _targetRotation;
            }
        }
        private void OnValidate() => Start();
    }
}