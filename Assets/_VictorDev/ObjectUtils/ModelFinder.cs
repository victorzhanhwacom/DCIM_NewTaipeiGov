using System.Collections.Generic;
using System.Linq;
using _VictorDev.ApiExtensions;
using _VictorDev.Configs;
using _VictorDev.InterfaceUtils;
using _VictorDev.DebugUtils;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Debug = _VictorDev.DebugUtils.Debug;

namespace _VictorDev.ObjectUtils
{
    /// 依關鍵字尋找模型
    public class ModelFinder : MonoBehaviour
    {
        #region Variables

        [Label("名稱關鍵字"), SerializeField] private string[] keyWords;
        [Label("尋獲的模型"), SerializeField] private List<Transform> foundModels;

        [Label("[接收器]"), SerializeField] private List<MonoBehaviour> receivers;


        [Foldout("[設定]"), SerializeField] private EnumSearchType searchType = EnumSearchType.Include;
        [Foldout("[設定]"), SerializeField] private Transform targetModelsParent;
        [Foldout("[設定]"), SerializeField, Label("設定LayerMask(選填)")] private LayerMask layerMask;
        
        #endregion


        [Button]
        public void FindModelsByKeywords()
        {
            foundModels = targetModelsParent.FindChildrenByKeywords(searchType, keyWords);
            receivers.Cast<IReceiveData<List<Transform>>>().ToList()
                .ForEach(receiver=> receiver.ReceiveData(foundModels));
            Debug.Log($"Found {foundModels.Count} target objects.", this);
        }
        
        [Button]
        public void SetLayerMask()
        {
            foundModels.ForEach(target => target.gameObject.SetLayerMask(layerMask));
            Debug.Log($"SetLayerMask is done.", this);
        }

        private void OnValidate() => receivers = ObjectHelper.CheckTypeOfList<IReceiveData<List<Transform>>>(receivers);

        
#if UNITY_EDITOR
        [Button]
        public void SelectObjects() => Selection.objects = foundModels.Select(t => t.gameObject).ToArray<Object>();
#endif
    }
}