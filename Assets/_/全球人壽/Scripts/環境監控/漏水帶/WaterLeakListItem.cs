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
        [Foldout("[Data]"), SerializeField] private RealtimeAsset_WaterLeak waterLeakData;
        [Foldout("[Events]")] public UnityEvent<int> onAlertLevelChanged;
        [Foldout("[Component]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        [Foldout("[Component]"), SerializeField] private DOTweenText txtValue;
        [Foldout("[Component]"), SerializeField] private Toggle toggle;

        public RealtimeAsset_WaterLeak WaterLeakData => waterLeakData;

        public void SetWaterLeakData(RealtimeAsset_WaterLeak data)
        {
            waterLeakData = data;
            txtDeviceName.text = data.deviceName;
            txtValue.text = waterLeakData.status;
            onAlertLevelChanged?.Invoke(waterLeakData.alertStatus);
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
