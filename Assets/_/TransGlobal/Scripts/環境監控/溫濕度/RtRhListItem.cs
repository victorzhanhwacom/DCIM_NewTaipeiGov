using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VzDev.DOTweenUtils;
using VzDev.InteractiveUtils.ModelMouseEvent;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 溫濕度數據綁定器：列表項目
    /// </summary>
    public class RtRhListItem : MonoBehaviour
    {
        [Foldout("[Data]"), SerializeField] private RealtimeAsset_RtRh rtrhData;
        [Foldout("[Component]"), SerializeField] private TextMeshProUGUI txtDeviceName;
        [Foldout("[Component]"), SerializeField] private DOTweenText txtRt, txtRh;
        [Foldout("[Component]"), SerializeField] private Toggle toggle;

        public RealtimeAsset_RtRh RtRhData => rtrhData;

        public void SetRtRhData(RealtimeAsset_RtRh data)
        {
            rtrhData = data;
            txtDeviceName.text = data.deviceName;
            txtRt.text = rtrhData.rtTag.value;
            txtRh.text = rtrhData.rhTag.value;
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
            if (isOn) ColliderInteractionSystem.SimulateClick(rtrhData.modelInfo.modelTarget.gameObject, ColliderInteractionSystem.ClickModelTrigger.byJsCall);
            //else if (toggle.group != null && toggle.group.AnyTogglesOn() == false) ColliderInteractionSystem.SimulateClickEmpty();
        }
    }
}
