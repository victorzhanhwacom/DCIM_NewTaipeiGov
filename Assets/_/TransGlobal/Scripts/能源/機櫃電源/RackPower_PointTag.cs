using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DOTweenUtils;

namespace VzDev.DCIMUtils
{
    public class RackPower_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_PowerPanel>
    {
        #region Components
        [Foldout("[Components]-Title"), SerializeField]
        private TextMeshProUGUI
       titleText_kW, titleText_kWH, titleText_vavg
       , titleText_ir, titleText_is, titleText_it
       , titleText_pf_r, titleText_pf_s, titleText_pf_t;

        [Foldout("[Components]-Value"), SerializeField]
        private DOTweenText
        dtweenText_kWH, dtweenText_vavg
        , dtweenText_ir, dtweenText_is, dtweenText_it
        , dtweenText_pf_r, dtweenText_pf_s, dtweenText_pf_t;
        #endregion

        #region UnityEvents
        [Foldout("[Event]"), SerializeField]
        private UnityEvent<string> kwValueEvent;
        [Foldout("[Event]-AlertStatus"), SerializeField]
        private UnityEvent<int>
      kwAlertStatusEvent, kWHAlertStatusEvent, vavgAlertStatusEvent
      , irAlertStatusEvent, isAlertStatusEvent, itAlertStatusEvent
      , pf_rAlertStatusEvent, pf_sAlertStatusEvent, pf_tAlertStatusEvent;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataPower.WebAPI_PowerPanelData);
            WebAPI_CallerBase_RealtimeDataPower.OnGetPowerPanelDataAction += OnGetDataAction;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataPower.OnGetPowerPanelDataAction -= OnGetDataAction;

        override protected void InvokeEvent()
        {
            base.InvokeEvent();
            titleText_kW?.SetText(data?.kWTag?.displayName);
            titleText_kWH?.SetText(data?.kWHTag?.displayName);
            titleText_vavg?.SetText(data?.vavgTag?.displayName);
            titleText_ir?.SetText(data?.i_rTag?.displayName);
            titleText_is?.SetText(data?.i_sTag?.displayName);
            titleText_it?.SetText(data?.i_tTag?.displayName);
            titleText_pf_r?.SetText(data?.pf_rTag?.displayName);
            titleText_pf_s?.SetText(data?.pf_sTag?.displayName);
            titleText_pf_t?.SetText(data?.pf_tTag?.displayName);

            dtweenText_ir?.SetText($"{data?.i_rTag?.value} {data?.i_rTag?.unit}");
            dtweenText_is?.SetText($"{data?.i_sTag?.value} {data?.i_sTag?.unit}");
            dtweenText_it?.SetText($"{data?.i_tTag?.value} {data?.i_tTag?.unit}");
            dtweenText_kWH?.SetText($"{data?.kWHTag?.value} {data?.kWHTag?.unit}");
            dtweenText_pf_r?.SetText($"{data?.pf_rTag?.value} {data?.pf_rTag?.unit}");
            dtweenText_pf_s?.SetText($"{data?.pf_sTag?.value} {data?.pf_sTag?.unit}");
            dtweenText_pf_t?.SetText($"{data?.pf_tTag?.value} {data?.pf_tTag?.unit}");
            dtweenText_vavg?.SetText($"{data?.vavgTag?.value} {data?.vavgTag?.unit}");

            kwValueEvent?.Invoke($"{data?.kWTag?.value} {data?.kWTag?.unit}");

            irAlertStatusEvent?.Invoke(data?.i_rTag?.alertLevelStatus ?? 2);
            isAlertStatusEvent?.Invoke(data?.i_sTag?.alertLevelStatus ?? 2);
            itAlertStatusEvent?.Invoke(data?.i_tTag?.alertLevelStatus ?? 2);
            kwAlertStatusEvent?.Invoke(data?.kWTag?.alertLevelStatus ?? 2);
            kWHAlertStatusEvent?.Invoke(data?.kWHTag?.alertLevelStatus ?? 2);
            pf_rAlertStatusEvent?.Invoke(data?.pf_rTag?.alertLevelStatus ?? 2);
            pf_sAlertStatusEvent?.Invoke(data?.pf_sTag?.alertLevelStatus ?? 2);
            pf_tAlertStatusEvent?.Invoke(data?.pf_tTag?.alertLevelStatus ?? 2);
            vavgAlertStatusEvent?.Invoke(data?.vavgTag?.alertLevelStatus ?? 2);
        }
    }
}