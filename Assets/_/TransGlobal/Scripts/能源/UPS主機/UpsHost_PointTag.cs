using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace VzDev.DCIMUtils
{
    public class UpsHost_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_UPSHost>
    {

        #region Events
        #endregion
         #region Fields
         [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtUpsMode;
        #endregion

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataPower.WebAPI_UpsHostData);
            WebAPI_CallerBase_RealtimeDataPower.OnGetUpsHostDataAction += OnGetDataAction;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataPower.OnGetUpsHostDataAction -= OnGetDataAction;
    }
}