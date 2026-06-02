using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VzDev.ApiExtensions
{
    /// 原API類別功能擴充
    public static class RaycastHitExtension
    {
        /// [Extended] - 複製一份轉為TransformList
        public static List<Transform> CloneToTransformList(this RaycastHit[] self) => self.CloneToTransformArray().ToList();

        /// [Extended] - 複製一份轉為Transform[]
        public static Transform[] CloneToTransformArray(this RaycastHit[] self, int size = 10)
        {
            Transform[] result = new Transform[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = self[i].collider.gameObject.transform;
            }
            return result;
        }

        /// [Extended] - 與ray.origin座標之距離進行排序 (是否遞減排序)
        public static void SortByOriginDistance(this RaycastHit[] self, int size, bool isDesc = false)
        {
            Array.Sort(self, 0, size, Comparer<RaycastHit>.Create(
                (a, b) => isDesc ? b.distance.CompareTo(a.distance) : a.distance.CompareTo(b.distance)
            ));
        }
    }
}