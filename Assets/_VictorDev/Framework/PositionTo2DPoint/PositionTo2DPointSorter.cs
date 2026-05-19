using System.Collections.Generic;
using System.Linq;
using _VictorDev.ApiExtensions;
using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.Framework
{
    /// 對PositionTo2DPoint進行前後排序
    public class PositionTo2DPointSorter : MonoBehaviour
    {
        [Label("[資料項 - PositionTo2DPoint]"), SerializeField] private List<PositionTo2DPoint> posTo2DPointList;

        private void Update()
        {
            // 根据攝影機距离对Landmark进行排序并调整Sibling Index
            posTo2DPointList.Sort((a, b) => b.DistanceFromCamera.CompareTo(a.DistanceFromCamera));
            for (int i = 0; i < posTo2DPointList.Count; i++)
            {
                posTo2DPointList[i].transform.SetSiblingIndex(i);
            }
        }
        
        public void AddToSortList(PositionTo2DPoint positionTo2DPoint) 
            => posTo2DPointList.ClearMissingTargets().TryAdd(positionTo2DPoint);
        
        [Button]
        private void GetLandmarksFromThisContainer() => posTo2DPointList = GetComponentsInChildren<PositionTo2DPoint>(true).ToList();
    }
}