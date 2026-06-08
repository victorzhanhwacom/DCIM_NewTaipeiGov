using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VzDev.DemoUtils.AutoJumper;
using VzDev.ObjectUtils;

public class PointTag_RTRH : MonoBehaviour
{
   [SerializeField, ReadOnly] private HeatSource heatSource;
   [Foldout("[Settings]"), SerializeField] private ColorRange[] colorRanges;
   [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtValue;
   [Foldout("[Components]"), SerializeField] private UIAnchorFollower uIAnchorFollower;
   [Foldout("[Components]"), SerializeField] private Image img;


    private void Start()
    {
        Debug.Log("PointTag_RTRH Start");
        if(uIAnchorFollower.Target3DObject.TryGetComponent(out ValueAutoJumper autoJumper))
            autoJumper.onValueChangedFloat.AddListener(SetHeatSource);

        if(uIAnchorFollower.Target3DObject.TryGetComponent<HeatSource>(out HeatSource heat))
            SetHeatSource(heat.temperature);

    }

    public void SetHeatSource(float value)
    {
        Debug.Log($"SetHeatSource: {value}");
        txtValue.SetText(value.ToString());
        foreach (var range in colorRanges)
        {
            if (value >= range.threshold)
            {
                img.color = range.color;
                break;
            }
        }
    }

    [Serializable]
    public struct ColorRange
    {
        public float threshold;
        public Color color;
    }
}
