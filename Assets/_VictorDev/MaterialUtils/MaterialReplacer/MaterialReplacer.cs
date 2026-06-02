using System.Collections.Generic;
using VzDev.InterfaceUtils;
using NaughtyAttributes;
using UnityEngine;

namespace VzDev.MaterialUtils
{
    /// 處理3D物件的材質替換
    public class MaterialReplacer: MonoBehaviour, IReceiveData<List<Transform>>
    {
        #region Variables

        [Label("[Target Models]"), SerializeField] private List<Transform> targetModels;
        [Label("[Exclude Models]"), SerializeField] private List<Transform> excludeModels;
        [Foldout("[Settings]"), SerializeField] private Material replaceMaterial;
        
        #endregion

        /// 設定目標模型
        public void SetTargetModels(List<Transform> models) => targetModels = models;

        /// 將目標模型材質替換為指定材質
        [Button]
        public void ReplaceModelsMaterial()
        {
            if(excludeModels != null && excludeModels.Count > 0)
            {
                MaterialHelper.ReplaceMaterial(targetModels, replaceMaterial, excludeModels);
            }
            else
            {
                MaterialHelper.ReplaceMaterial(targetModels, replaceMaterial);
            }
        }

        /// 將材質恢復為原始材質
        [Button]
        public void RestoreModelsMaterial() => MaterialHelper.RestoreMaterial(targetModels);

        public void ReceiveData(List<Transform> models)
        {
            targetModels = models;
        }
    }
}