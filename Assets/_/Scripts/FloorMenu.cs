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
    [Foldout("[Settings]"), SerializeField] private float distance = 30f;
    #endregion

    public void SwitchToFollor(string floor)
    {
        bool isTargetFloor;
        for(int i = 0; i < floorItems.Count; i++)
        {
            isTargetFloor = floorItems[i].floor.Trim() == floor.Trim();
            if (isTargetFloor)
            {
                Transform anchor = floorItems[i].anchor;
                if (anchor != null)
                {
                    onFloorSelected?.Invoke(anchor, distance);
                }
            }
            floorItems[i].floorObjects?.ForEach(obj => obj.gameObject.SetActive(isTargetFloor));
        }

        /* Transform anchor = floorItems.Find(item => item.floor.Trim() == floor.Trim()).anchor;
        if (anchor != null)
        {
            onFloorSelected?.Invoke(anchor, distance);
        } */
    }

    [Serializable]
    public struct FloorAnchorItem
    {
        public string floor;
        public Transform anchor;
        public List<Transform> floorObjects;
    }
}
