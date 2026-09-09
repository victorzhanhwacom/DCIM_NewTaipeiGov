using UnityEngine;
using UnityEngine.Events;
using VzDev.ObjectUtils;

public class TEMP_VESP_ForceAlert : MonoBehaviour
{
    public UIAnchorFollower anchorFollower;

    public UnityEvent onAlert;

    private void OnEnable()
    {
        if (anchorFollower.Target3DObject == null) return;
        if (anchorFollower.Target3DObject.name.Contains("Alert"))
        {
            onAlert?.Invoke();
        }
    }
}
