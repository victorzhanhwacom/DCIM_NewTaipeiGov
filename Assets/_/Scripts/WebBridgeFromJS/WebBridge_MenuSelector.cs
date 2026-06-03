using System;
using System.Collections.Generic;
using System.Diagnostics;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class WebBridge_MenuSelector : MonoBehaviour
{
    #region Variables

    [SerializeField, ReadOnly] private EnumMainMenu ReceiveMenuName;
    [SerializeField] private MenuSelectorItem[] menuSelectorItems;

    private Dictionary<EnumMainMenu, MenuSelectorItem> dictMenuSelectorItems = new Dictionary<EnumMainMenu, MenuSelectorItem>();

    private bool IsNotPlaying => !Application.isPlaying;

    #endregion

    private void OnValidate() => name = GetType().Name;

    private void Awake()
    {
        // 初始化字典，將 Enum 名稱（字串）對應到 MenuSelectorItem
        foreach (var item in menuSelectorItems)
        {
            if (!dictMenuSelectorItems.TryAdd(item.enumMainMenu, item))
            {
                Debug.LogWarning($"[WebBridge_MenuSelector] 警告：Key '{item.enumMainMenu}' 已經存在於字典中，無法添加。");
            }
        }
    }
    public void CancelAllMenu()
    {
        for (int i = 0; i < menuSelectorItems.Length; i++)
        {
            menuSelectorItems[i].SetToggleIsOn(false);
        }
    }

    public void SelectMenu(string menuName)
    {
        // 1. 將網頁傳來的字串轉換為 Enum（忽略大小寫差異，填入 true）
        if (Enum.TryParse(menuName, true, out ReceiveMenuName))
        {
            Debug.Log($"[WebBridge] 成功收到選單指令: {menuName}，對應的 Index 為: {(int)ReceiveMenuName}");

            if(ReceiveMenuName == EnumMainMenu.None)
            {
                CancelAllMenu();
                return;
            }

            // 3. 執行您原本切換選單或功能的邏輯
            dictMenuSelectorItems[ReceiveMenuName].SetToggleIsOn(true); // 例如：切換到對應的 Toggle
        }
        else
        {
            // 防呆機制：如果網頁端傳了 Enum 列表以外的字串
            Debug.LogError($"[WebBridge] 錯誤：網頁端傳來了未定義的選單名稱 -> '{menuName}'");
        }
    }

#if UNITY_EDITOR
    [Button(), ShowIf(nameof(IsNotPlaying))]
    private void FindMenuSelectorItem() => menuSelectorItems = FindObjectsByType<MenuSelectorItem>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#endif
}

public enum EnumMainMenu
{
    None = -1,
    Power = 0,
    Enviornment = 1,
    CCTV = 2,
    Door = 3,
    BMS = 4,
    ICT = 5,
    Deployment = 6,
    Alarm = 7,
}

