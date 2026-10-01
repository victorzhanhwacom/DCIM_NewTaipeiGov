using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VzDev.DOTweenUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 漏水帶數據綁定器：列表項目
    /// </summary>
    public class WaterLeakListItem : MonoBehaviour
    {
        [Foldout("[Data]"), SerializeField] private WebAPI_RealtimeData_WaterLeak waterLeakData;
        [Foldout("[Events]")] public UnityEvent<int> onAlertLevelChanged;
        [Foldout("[Component]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        [Foldout("[Component]"), SerializeField] private DOTweenText txtValue;
        [Foldout("[Component]"), SerializeField] private Toggle toggle;

        public WebAPI_RealtimeData_WaterLeak WaterLeakData => waterLeakData;

        public void SetWaterLeakData(WebAPI_RealtimeData_WaterLeak data)
        {
            waterLeakData = data;
            txtDeviceName.SetText(waterLeakData.deviceName);
            txtValue?.SetText(waterLeakData.value);
            onAlertLevelChanged?.Invoke(waterLeakData.TotalAlertLevelStatus);
        }

        public void SetToggleGroup(ToggleGroup group) => toggle.group = group;

        public void OnEnable()
        {
            if (toggle != null) toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        public void OnDisable()
        {
            if (toggle != null) toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn) ColliderInteractionSystem.SimulateClick(waterLeakData.modelInfo.modelTarget.gameObject, ColliderInteractionSystem.ClickModelTrigger.byJsCall);
            //else if (toggle.group != null && toggle.group.AnyTogglesOn() == false) ColliderInteractionSystem.SimulateClickEmpty();
        }
    }
}
