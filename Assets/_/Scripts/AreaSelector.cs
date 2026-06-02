using System;
using System.Linq;
using VzDev.ApiExtensions;
using VzDev.CameraUtils;
using TMPro;
using UnityEngine;

public class AreaSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public RTSCameraController camera;

    public Transform[] landMarkPos;

    public float camDistance = 35;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int index) => ToPosition(dropdown.options[index].text.Split(" ")[0]);

    public void ToPosition(string areaName)
    {
        Transform target = landMarkPos.FirstOrDefault(x => x.name.ContainKeyword(StringComparison.OrdinalIgnoreCase, areaName));
        if(target!=null) camera.FlyToPosition(target);
    }
}
