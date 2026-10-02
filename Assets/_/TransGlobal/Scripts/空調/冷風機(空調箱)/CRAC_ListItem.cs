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
    [Foldout("[Event]"), SerializeField] private UnityEvent<int> totalAlertLevelStatusEvent;
    /// <summary>
    /// 手動控制狀態事件: 啟動(true), 停止(false)
    /// </summary>
    [Foldout("[Event]"), SerializeField, Tooltip("手動控制狀態事件")] private UnityEvent<bool> manualStatusEvent;
    #endregion
    #region Fields
    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName, txtTempSet;
    [Foldout("[Components]"), SerializeField] private DOTweenText txtRoomTemp;
    [Foldout("[Components]-手動啟動"), SerializeField] private Button btnManualToOn, btnManualToOff;
    [Foldout("[Components]-設定溫度"), SerializeField] private Button btnTempIncrease, btnTempDecrease;
    private float? tempSetValue;

    public string bodyJson_SetTemp;
    #endregion

    #region Event Listener
    protected override void OnEnable()
    {
        base.OnEnable();
        btnManualToOn.onClick.AddListener(OnClickManualToOn);
        btnManualToOff.onClick.AddListener(OnClickManualToOff);
        btnTempIncrease.onClick.AddListener(OnBtnTempSetIncrease);
        btnTempDecrease.onClick.AddListener(OnBtnTempSetDecrease);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        btnTempIncrease.onClick.RemoveListener(OnBtnTempSetIncrease);
        btnTempDecrease.onClick.RemoveListener(OnBtnTempSetDecrease);
    }
    #endregion

    #region 手動啟動 / 關閉
    private void OnClickManualToOn() => SetManualControlValue(true);
    private void OnClickManualToOff() => SetManualControlValue(false);
    private void SetManualControlValue(bool isOn)
    {
        Debug.Log($"SetManualControlValue: {isOn}");
        manualStatusEvent?.Invoke(isOn);
    }
    #endregion

    #region 設定溫度
    private void OnBtnTempSetIncrease() => SetTempValue(0.1f);
    private void OnBtnTempSetDecrease() => SetTempValue(-0.1f);
    private void SetTempValue(float adjustValue)
    {
        tempSetValue ??= data.tempSetTag.value == "---" ? float.Parse(data.rtTag.value) : float.Parse(data.tempSetTag.value);
        tempSetValue += adjustValue;
        txtTempSet.SetText(tempSetValue?.ToString("0.##"));

        // 隔2秒後才將最終的溫度值送出，避免使用者連續點擊造成頻繁送出，過程中若使用者再次點擊則會重新計算2秒後送出
        DeviceControl deviceControl = new DeviceControl(data.tempSetTag.tagId, tempSetValue ?? 0f);

    }
    #endregion

    /// <summary>
    /// 手動控制開關值改變事件
    /// </summary>
    private void OnToggleManualControlValueChanged(bool isOn)
    {
        Debug.Log($"OnToggleManualControlValueChanged: {isOn}");
    }

    protected override void UpdateUI(WebAPI_RealtimeData_CRAC data)
    {
        txtDeviceName.SetText(data.deviceName);
        txtTempSet.SetText($"{data.tempSetTag.value} {data.tempSetTag.unit}");
        txtRoomTemp.SetText($"{data.rtTag.value} {data.rtTag.unit}");

        totalAlertLevelStatusEvent?.Invoke(data.TotalAlertLevelStatus);
        manualStatusEvent?.Invoke(data.manualControlStatus);
    }
}