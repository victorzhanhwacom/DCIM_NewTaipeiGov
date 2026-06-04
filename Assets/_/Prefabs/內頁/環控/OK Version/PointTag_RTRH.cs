using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VzDev.DemoUtils.AutoJumper;
using VzDev.ObjectUtils;

public class PointTag_RTRH : MonoBehaviour
{
   [SerializeField, ReadOnly] private HeatSource heatSource;
   [Foldout("[Settings]"), SerializeField] private Color normalColor = Color.green, alarmColor = Color.yellow, warningColor = Color.red;
   [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtValue;
   [Foldout("[Components]"), SerializeField] private UIAnchorFollower uIAnchorFollower;
   [Foldout("[Components]"), SerializeField] private Image img;


    private void Start()
    {
        if(uIAnchorFollower.Target3DObject.TryGetComponent<ValueAutoJumper>(out ValueAutoJumper autoJumper))
            autoJumper.onValueChangedFloat.AddListener(SetHeatSource);

        if(uIAnchorFollower.Target3DObject.TryGetComponent<HeatSource>(out HeatSource heat))
            SetHeatSource(heat.temperature);

    }

    public void SetHeatSource(float value)
    {
        txtValue.SetText(value.ToString());
        if(value > 27f || value < 18f)
            img.color = warningColor;
        else if(value > 25f)
            img.color = alarmColor;
        else
            img.color = normalColor;
    }
}
