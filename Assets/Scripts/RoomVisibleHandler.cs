using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class RoomVisibleHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<RoomItem> roomItems;
    
    private Dictionary<EnumRoomName, RoomItem> _roomItemDictionary;
    private List<Transform> _allRoomModels;
    #endregion

    private void Start() => InitializedData();
    private void InitializedData()
    {
        _roomItemDictionary = roomItems.ToDictionary(x => x.RoomName, x => x);
        _allRoomModels = roomItems.SelectMany(x => x.Models).ToList();
    }

    public EnumRoomName RoomName;
    [Button]
    public void ShowRoomOnly() => ShowRoomOnly(RoomName);
    
    
    [Button]
    public void ShowRoomOnly(EnumRoomName roomName)
    {
        InitializedData();
        Transform[] targetModels = _roomItemDictionary[roomName].Models;

        for (int i = 0; i < _allRoomModels.Count; i++)
        {
            Transform model = _allRoomModels[i];
            model.gameObject.SetActive(targetModels.Contains(model));
        }
    }
    
    public struct RoomItem
    {
        public EnumRoomName RoomName;
        public Transform[] Models;
    }

    public enum EnumRoomName
    {
        多元會議室, 會議室1, 會議室2, 辦公區A, 辦公區B, 局長辦公室, 副局長辦公室, 主控室, AI機房, 機房A, 機房B
    }
}
