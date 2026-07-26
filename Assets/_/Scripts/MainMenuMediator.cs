using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VzDev;

public class MainMenuMediator : MonoBehaviour
{
    [Foldout("[Components]"), SerializeField] private TMP_Dropdown dropdownMainMenu;
    [Foldout("[Components]"), SerializeField] private MaterialReplacerMediator materialReplacerMediator;

    private void OnEnable() => dropdownMainMenu.onValueChanged.AddListener(SetMainMenuIndex);
    private void OnDisable() => dropdownMainMenu.onValueChanged.RemoveListener(SetMainMenuIndex);

    public void SetMainMenuIndex(int value)
    {
        switch (value)
        {
            case 0:
                materialReplacerMediator.SetPowerModelVisible(true);
                break;
            case 1:
                materialReplacerMediator.SetEnvModelVisible(true);
                break;
            case 2:
                materialReplacerMediator.SetCCTVModelVisible(true);
                break;
            case 3:
                materialReplacerMediator.SetDoorModelVisible(true);
                break;
            case 4:
                materialReplacerMediator.SetACModelVisible(true);
                break;
            case 5:
                materialReplacerMediator.SetCabinetModelVisible(true);
                break;
        }
    }
}
