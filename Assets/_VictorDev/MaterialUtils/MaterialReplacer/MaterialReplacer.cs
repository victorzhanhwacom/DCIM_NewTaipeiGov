using System.Collections.Generic;
using _VictorDev.InterfaceUtils;
using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.MaterialUtils
{
    /// 處理3D物件的材質替換
    public class MaterialReplacer: MonoBehaviour, IReceiveData<List<Transform>>
    {
        #region Variables

        [Label("[模型列表]"), SerializeField] private List<Transform> targetModels;
        [Foldout("[設定]"), SerializeField] private Material replaceMaterial;
        
        #endregion

        /// 設定目標模型
        public void SetTargetModels(List<Transform> models) => targetModels = models;
        
        /// 將目標模型材質替換為指定材質
        [Button]
        public void ReplaceModelsMaterial() => MaterialHelper.ReplaceMaterial(targetModels, replaceMaterial);
        
        /// 將材質恢復為原始材質
        [Button]
        public void RestoreModelsMaterial() => MaterialHelper.RestoreMaterial(targetModels);

        public void ReceiveData(List<Transform> models)
        {
            targetModels = models;
        }
    }
}