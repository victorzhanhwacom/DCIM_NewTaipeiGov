using System;
using UnityEngine;

namespace VzDev.DCIM.Deployment
{
    /// <summary>
    /// DCIM模型資料
    /// </summary>
    [Serializable]
    public class ModelInfo
    {
        /// <summary>
        /// 模型對像Prefab
        /// </summary>
        public Transform modelTarget;

        public void SetModelTarget(Transform model) => modelTarget = model;
    }
}