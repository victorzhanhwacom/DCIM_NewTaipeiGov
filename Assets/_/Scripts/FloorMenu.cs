using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class FloorMenu : MonoBehaviour
{
    #region Fileds
    [Foldout("[Events]")] public UnityEvent<Transform, float> onFloorSelected;
    [SerializeField] private List<FloorAnchorItem> floorItems;
    #endregion

    public void SwitchToFollor(string floor)
    {
        bool isTargetFloor;
        for (int i = 0; i < floorItems.Count; i++)
        {
            isTargetFloor = floorItems[i].floor.Trim() == floor.Trim();
            if (isTargetFloor)
            {
                floorItems[i].onSelected?.Invoke();
                Transform anchor = floorItems[i].anchor;
                if (anchor != null)
                {
                    onFloorSelected?.Invoke(anchor, floorItems[i].distance);
                }
            }
            floorItems[i].isSelected?.Invoke(isTargetFloor);
        }
    }

    [Serializable]
    public struct FloorAnchorItem
    {
        public string floor;
        public Transform anchor;
        public float distance;

        public UnityEvent onSelected;
        public UnityEvent<bool> isSelected;
    }
}
