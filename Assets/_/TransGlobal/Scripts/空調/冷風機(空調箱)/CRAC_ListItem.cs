using System.Collections.Generic;
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
    [Foldout("[Event]-Value"), SerializeField] private UnityEvent<string> powerValueEvent, manualControlValueEvent;
    [Foldout("[Event]-Status"), SerializeField] private UnityEvent<bool> powerStatusEvent, manualControlStatusEvent;
    [Foldout("[Event]-AlertLevelStatus"), SerializeField] private UnityEvent<int> rtAlertLevelStatusEvent, alarmStatusEvent;
    [Foldout("[Event]-MainControl"), SerializeField] private UnityEvent<bool> isAutoControlEvent;

    #endregion

    #region Fields
    [Foldout("[Component]"), SerializeField] private DOTweenText dotRt;
    [Foldout("[Component]"), SerializeField] private TextMeshProUGUI txtTempSet, txtDeviceName;
    [Foldout("[Component]"), SerializeField] private Button btnDecreaseTemp, btnIncreaseTemp, btnManualControl;
    [Foldout("[Component]"), SerializeField] private GameObject manualControlLoadingBar;
    private float tempMin = 16f, tempMax = 30f;
    private float invokeTempSetAfterSeconds = 2f;
    private float currentTempSet;
    #endregion

    #region Event Listener
    protected override void OnEnable()
    {
        base.OnEnable();
        btnIncreaseTemp.onClick.AddListener(IncreaseTemp);
        btnDecreaseTemp.onClick.AddListener(DecreaseTemp);
        btnManualControl.onClick.AddListener(ToggleManualControl);
        CRAC_MainController.OnManualControlStatusChangedAction += OnManualControlStatusChanged;
    }
    private void OnManualControlStatusChanged(string controlRoom, bool isAutoControlMode)
    {
        if (data == null) return;
        if (data.controllRoom != controlRoom) return;
        isAutoControlEvent?.Invoke(isAutoControlMode);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        btnIncreaseTemp.onClick.RemoveListener(IncreaseTemp);
        btnDecreaseTemp.onClick.RemoveListener(DecreaseTemp);
        btnManualControl.onClick.RemoveListener(ToggleManualControl);
        CRAC_MainController.OnManualControlStatusChangedAction -= OnManualControlStatusChanged;
    }
    #endregion

    #region 設定溫度
    public void IncreaseTemp() => SetTemp(0.5f);
    public void DecreaseTemp() => SetTemp(-0.5f);
    private void SetTemp(float adjustValue)
    {
        if (data == null) return;
        if (data.tempSetTag == null) return;

        currentTempSet += adjustValue;

        // 限制溫度範圍
        currentTempSet = Mathf.Clamp(currentTempSet, tempMin, tempMax);
        txtTempSet.SetText($"{currentTempSet} {data.tempSetTag.unit}");

        btnIncreaseTemp.interactable = currentTempSet < tempMax;
        btnDecreaseTemp.interactable = currentTempSet > tempMin;

        //在設定完之後，延遲一段時間再呼叫API，避免連續快速點擊造成API過度呼叫
        CancelInvoke(nameof(InvokeSetTemp));
        Invoke(nameof(InvokeSetTemp), invokeTempSetAfterSeconds);
    }

    /// <summary>
    /// 呼叫API設定新的溫度
    /// </summary>
    private void InvokeSetTemp()
    {
        void OnError(string error)
        {
            Debug.LogError($"[CRAC_PointTag] InvokeSetTemp Error: {error}");
        }
        void OnSuccess(List<DeviceControlResult> list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning($"[CRAC_PointTag] InvokeSetTemp OnSuccess: list is null or empty.");
                return;
            }
            var result = list[0];
            if (result.IsSuccess)
            {
                Debug.Log($"[CRAC_PointTag] 設定溫度成功: {currentTempSet}");
                data.tempSetTag.SetValue(currentTempSet.ToString());
                DOTweenHelper.ToBlink(txtTempSet, $"{data.tempSetTag.value} {data.tempSetTag.unit}");
            }
            else
                Debug.LogWarning($"[CRAC_PointTag] InvokeSetTemp Error:\ntagId={result.tagId}\nstatus={result.status}\nerror={result.error}");
        }

        // 呼叫API設定新的溫度e
        WebAPI_CallerBase_DeviceControl.SetDeviceControl(data.controlTag.tagId, currentTempSet, OnSuccess, OnError);
    }
    #endregion

    #region 手動啟動/關閉
    private void ToggleManualControl()
    {
        if (data == null) return;
        if (data.controlTag == null) return;

        bool newManualControlStatus = !data.manualControlStatus;

        void OnError(string error)
        {
            manualControlLoadingBar.SetActive(false);
            Debug.LogError($"[CRAC_PointTag] ToggleManualControl Error: {error}");
        }
        void OnSuccess(List<DeviceControlResult> list)
        {
            manualControlLoadingBar.SetActive(false);
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning($"[CRAC_PointTag] ToggleManualControl OnSuccess: list is null or empty.");
                return;
            }
            var result = list[0];
            if (result.IsSuccess)
            {
                Debug.Log($"[CRAC_PointTag] 設定手動啟動狀態成功");
                data.controlTag.SetValue(newManualControlStatus ? "啟動" : "停止");
                manualControlValueEvent?.Invoke(data.controlTag.value);
                manualControlStatusEvent?.Invoke(data.manualControlStatus);
            }
            else
                Debug.LogWarning($"[CRAC_PointTag] ToggleManualControl Error:\ntagId={result.tagId}\nstatus={result.status}\nerror={result.error}");
        }

        manualControlLoadingBar.SetActive(true);
        // 呼叫API設定新的手動啟動狀態
        WebAPI_CallerBase_DeviceControl.SetDeviceControl(data.controlTag.tagId, newManualControlStatus, OnSuccess, OnError);
    }
    #endregion

    protected override void UpdateUI(WebAPI_RealtimeData_CRAC data)
    {
        currentTempSet = float.Parse(data.tempSetTag.value);

        powerValueEvent?.Invoke(data.powerStatusTag.value);
        powerStatusEvent?.Invoke(data.powerStatus);

        txtDeviceName.SetText(data.deviceName);
        dotRt.SetText($"{data.rtTag.value} {data.rtTag.unit}");
        rtAlertLevelStatusEvent?.Invoke(data.rtTag.alertLevel);

        txtTempSet.SetText($"{data.tempSetTag.value} {data.tempSetTag.unit}");

        alarmStatusEvent?.Invoke(data.AlarmStatus);

        manualControlValueEvent?.Invoke(data.controlTag.value);
        manualControlStatusEvent?.Invoke(data.manualControlStatus);
    }
}