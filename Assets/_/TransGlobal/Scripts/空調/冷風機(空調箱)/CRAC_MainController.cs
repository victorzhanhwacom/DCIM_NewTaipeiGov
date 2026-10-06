using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VzDev.DCIMUtils;

/// <summary>
/// 空調箱 主控制器： 手動模式 / 自動模式
/// <para> + 若為自動模式時，相關的空調箱則不會被手動控制啟動/停止</para>
/// </summary>
public class CRAC_MainController : MonoBehaviour
{
    private enum MainControlRoom
    {
        電信室 = 0,
        電力室 = 1
    }

    #region Events
    /// <summary>
    /// 手動模式狀態改變事件
    /// <para>參數1：控制室名稱</para>
    /// <para>參數2：是否為手動模式</para>
    /// </summary>
    public static Action<string, bool> OnManualControlStatusChangedAction;
    [Foldout("[Event]"), SerializeField] private UnityEvent<int> isAutoControllEvent;
    #endregion

    #region Fields
    [SerializeField] private MainControlRoom controlRoom;
    [SerializeField, ReadOnly] private WebAPI_RealtimeData_CRAC_MainController data;
    [Foldout("[Component]"), SerializeField] private Button btnManualControl;
    [Foldout("[Component]"), SerializeField] private GameObject manualControlLoadingBar;
    #endregion

    #region Event Listener
    private void OnEnable()
    {
        OnGetDataAction(WebAPI_CallerBase_RealtimeDataHVAC.WebAPI_CRAC_MainControllerData);
        WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRAC_MainControllerDataAction += OnGetDataAction;
        btnManualControl.onClick.AddListener(ToggleManualControl);
    }
    private void OnGetDataAction(List<WebAPI_RealtimeData_CRAC_MainController> dataList)
    {
        if (dataList == null || dataList.Count == 0)
        {
            Debug.LogWarning($"[CRAC_MainController] OnGetDataAction: dataList is null or empty.");
            return;
        }
        data = dataList?.FirstOrDefault(data => data.controllRoom == controlRoom.ToString());
        isAutoControllEvent?.Invoke(data.IsAutoControlMode ? 1 : 0);
        OnManualControlStatusChangedAction?.Invoke(data.controllRoom, data.IsAutoControlMode);

    }

    private void OnDisable()
    {
        WebAPI_CallerBase_RealtimeDataHVAC.OnGetCRAC_MainControllerDataAction -= OnGetDataAction;
        btnManualControl.onClick.RemoveListener(ToggleManualControl);
    }
    #endregion

    #region 手動啟動/關閉
    private void ToggleManualControl()
    {
        if (data == null) return;

        bool autoControlMode = !data.IsAutoControlMode;

        void OnError(string error)
        {
            manualControlLoadingBar.SetActive(false);
            Debug.LogError($"[CRAC_MainController] ToggleManualControl Error: {error}");
        }
        void OnSuccess(List<DeviceControlResult> list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning($"[CRAC_MainController] ToggleManualControl OnSuccess: list is null or empty.");
                return;
            }
            var result = list[0];
            if (result.IsSuccess)
            {
                Debug.Log($"[CRAC_MainController] 設定手動啟動狀態成功: {autoControlMode}");
                data.tags[0].SetValue(autoControlMode ? "自動模式" : "手動模式");
                OnManualControlStatusChangedAction?.Invoke(data.controllRoom, data.IsAutoControlMode);
                isAutoControllEvent?.Invoke(autoControlMode ? 1 : 0);
            }
            else
                Debug.LogWarning($"[CRAC_MainController] ToggleManualControl Error:\ntagId={result.tagId}\nstatus={result.status}\nerror={result.error}");
            manualControlLoadingBar.SetActive(false);
        }

        manualControlLoadingBar.SetActive(true);
        // 呼叫API設定新的手動啟動狀態
        WebAPI_CallerBase_DeviceControl.SetDeviceControl(data.tags[0].tagId, autoControlMode, OnSuccess, OnError);
    }
    #endregion


}
