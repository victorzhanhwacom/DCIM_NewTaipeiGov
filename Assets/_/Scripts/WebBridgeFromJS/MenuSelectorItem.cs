using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelectorItem : MonoBehaviour
{
    #region 
    public EnumMainMenu enumMainMenu;
    [Foldout("[Settings]"), SerializeField] private string prefix = "MainMenuBtn_";
    [Foldout("[Settings]"), SerializeField] private Toggle toggle;
    #endregion

    public void SetToggleIsOn(bool isOn) => toggle.isOn = isOn;

    private void Awake() => OnValidate();

    private void OnValidate()
    {
        name = $"{prefix}{enumMainMenu}";
        if (toggle == null)
            toggle = GetComponent<Toggle>();
    }
}
