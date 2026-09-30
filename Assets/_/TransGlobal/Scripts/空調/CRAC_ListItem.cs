using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VzDev.DCIMUtils;
using VzDev.DOTweenUtils;
using VzDev.Frameworks.ScrollRectUtils;

public class CRAC_ListItem : ScrollRectListItemBase<WebAPI_RealtimeData_CRAC>
{
    #region UnityEvent
    [Foldout("[Event]-設定溫度"), SerializeField] private UnityEvent setTempValueEvent;
    [Foldout("[Event]-設定溫度"), SerializeField] private UnityEvent onClickTempControlButtonEvent;
    #endregion
    #region Fields
    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName, txtTempSet;
    [Foldout("[Components]"), SerializeField] private DOTweenText txtRoomTemp;
    [Foldout("[Components]"), SerializeField] private Toggle toggleManualControl;
    [Foldout("[Components]"), SerializeField] private Button btnTempIncrease, btnTempDecrease;
    private float? tempSetValue;

    public string bodyJson_SetTemp;
    #endregion

    #region Event Listener
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
    #endregion

    #region 設定溫度
    private void OnBtnTempSetIncrease() => SetTempValue(0.1f);
    private void OnBtnTempSetDecrease() => SetTempValue(-0.1f);
    private void SetTempValue(float adjustValue)
    {
        tempSetValue ??= string.IsNullOrEmpty(data.tempSetTag.value) ? float.Parse(data.rtTag.value) : float.Parse(data.tempSetTag.value);
        tempSetValue += adjustValue;
        txtTempSet.SetText(tempSetValue?.ToString("0.##"));

        DeviceControl deviceControl = new DeviceControl(data.tempSetTag.tagId, tempSetValue ?? 0f);

        setTempValueEvent?.Invoke();
        onClickTempControlButtonEvent?.Invoke();
    }
    #endregion

    /// <summary>
    /// 手動控制開關值改變事件
    /// </summary>
    private void OnToggleManualControlValueChanged(bool isOn)
    {
    }

    protected override void UpdateUI(WebAPI_RealtimeData_CRAC data)
    {
        txtDeviceName.SetText(data.deviceName);
        txtTempSet.SetText($"{data.tempSetTag.value} {data.tempSetTag.unit}");
        txtRoomTemp.SetText($"{data.rtTag.value} {data.rtTag.unit}");
        toggleManualControl.SetIsOnWithoutNotify(data.controlTag.value == "1");
    }
}