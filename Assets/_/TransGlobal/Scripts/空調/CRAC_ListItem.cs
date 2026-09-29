using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VzDev.DCIMUtils;
using VzDev.DOTweenUtils;
using VzDev.Frameworks.ScrollRectUtils;

public class CRAC_ListItem : ScrollRectListItemBase<WebAPI_RealtimeData_CRAC>
{
    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName, txtTempSet;
    [Foldout("[Components]"), SerializeField] private DOTweenText txtRoomTemp;
    [Foldout("[Components]"), SerializeField] private Toggle toggleManualControl;
    [Foldout("[Components]"), SerializeField] private Button btnTempIncrease, btnTempDecrease;

    protected override void OnEnable()
    {
        base.OnEnable();
        toggleManualControl.onValueChanged.AddListener(OnToggleManualControlValueChanged);
        btnTempIncrease.onClick.AddListener(OnBtnTempSetIncrease);
        btnTempDecrease.onClick.AddListener(OnBtnTempSetDecrease);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        toggleManualControl.onValueChanged.RemoveListener(OnToggleManualControlValueChanged);
        btnTempIncrease.onClick.RemoveListener(OnBtnTempSetIncrease);
        btnTempDecrease.onClick.RemoveListener(OnBtnTempSetDecrease);
    }

    private void OnBtnTempSetIncrease()
    {
    }

    private void OnBtnTempSetDecrease()
    {
    }

    private void OnToggleManualControlValueChanged(bool isOn)
    {
    }

    protected override void UpdateUI(WebAPI_RealtimeData_CRAC data)
    {
        txtDeviceName.SetText(data.deviceName);
        txtTempSet.SetText(data.tempSetTag.value);
        txtRoomTemp.SetText(data.rtTag.value);
        toggleManualControl.SetIsOnWithoutNotify(data.controlTag.value == "1");
    }
}