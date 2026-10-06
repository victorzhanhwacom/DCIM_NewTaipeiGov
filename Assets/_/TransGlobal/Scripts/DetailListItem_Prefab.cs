using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DOTweenUtils;
using static VzDev.DCIMUtils.WebAPI_RealtimeData;

public class DetailListItem : MonoBehaviour
{
    public UnityEvent<int> alertLevelStatusEvent;
    public TextMeshProUGUI txtDisplayName;
    public DOTweenText dtValue;

    public void SetData(Tag tag) => SetData(tag.displayName, tag.valueWithUnit, tag.alertLevelStatus);
    public void SetData(string displayName, string value, int alertLevelStatus)
    {
        txtDisplayName.text = displayName;
        if(dtValue.text != value) dtValue.SetText(value);
        alertLevelStatusEvent?.Invoke(alertLevelStatus);
    }
}
