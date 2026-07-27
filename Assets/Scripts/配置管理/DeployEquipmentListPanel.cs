using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeployDevicePanel : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField, ReadOnly] private Toggle[] toggles;

    public UnityEvent<Toggle> onToggleSelected;

    private void Start() => GetToggles();

    private void OnEnable()
    {
        for(int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[index].onValueChanged.AddListener(isOn => { if (isOn) onToggleSelected?.Invoke(toggles[index]); });
        }
    }
    private void OnDisable()
    {
        for(int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[index].onValueChanged.RemoveAllListeners();
        }
    }

    [Button]
    private void GetToggles()
    {
        toggles = scrollRect.content.GetComponentsInChildren<Toggle>(true);
    }
}
