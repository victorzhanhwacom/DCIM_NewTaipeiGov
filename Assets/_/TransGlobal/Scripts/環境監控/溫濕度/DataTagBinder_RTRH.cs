using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.ObjectUtils;
using static VzDev.DCIMUtils.EnviornmentUtils.RtRhDataManager;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 溫濕度數據綁定器：標籤
    /// <para> 透過DataModelBinder_RTRH取得溫濕度數據與溫濕度模式 </para>
    /// </summary>
    public class DataTagBinder_RTRH : MonoBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private EnumRtRhMode rtRhMode = EnumRtRhMode.Unselect;
        [SerializeField, ReadOnly] private RealtimeAsset_RtRh rtrhData;

        [Foldout("[Events]-Value")] public UnityEvent<string> onRtValueChangedEvent, onRhValueChangedEvent;
        [Foldout("[Events]")] public UnityEvent<bool> OnRtModeEvent, onRhModeEvent;
        [Foldout("[Components]"), SerializeField] private UIAnchorFollower uiAnchorFollower;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName, txtCategory;
        private DataModelBinder_RTRH dataModelBinder_RTRH;
        #endregion

        private void Awake()
        {
            if (transform.TryGetComponent(out uiAnchorFollower) == false)
            {
                Debug.LogWarning($"DataTagBinder_RTRH: UIAnchorFollower component not found on the GameObject.", this);
            }
        }
        private void OnValidate() => Awake();

        /// <summary>
        /// 各項值的處理
        /// </summary>
        private void ValueHandler()
        {
            txtDeviceName?.SetText(rtrhData.deviceName);
            txtCategory?.SetText(rtrhData.category.ToString());
            onRtValueChangedEvent?.Invoke(rtrhData.rtTag.value);
            onRhValueChangedEvent?.Invoke(rtrhData.rhTag.value);
        }

        /// <summary>
        /// 值改變時
        /// </summary>
        private void OnRtRhDataChangedAction(RealtimeAsset_RtRh data)
        {
            rtrhData = data;
            ValueHandler();
        }

        /// <summary>
        /// 改變溫濕度模式時
        /// </summary>
        private void OnRtRhTypeChanged(EnumRtRhMode mode)
        {
            rtRhMode = mode;
            OnRtModeEvent?.Invoke(rtRhMode == EnumRtRhMode.Rt);
            onRhModeEvent?.Invoke(rtRhMode == EnumRtRhMode.Rh);
        }

        #region Event Listeners
        private void OnEnable()
        {
            if (dataModelBinder_RTRH == null)
            {
                if (uiAnchorFollower.Target3DObject != null)
                {
                    uiAnchorFollower.Target3DObject.GetChild(0).TryGetComponent(out dataModelBinder_RTRH);

                }
                return;
            }
            else
            {
                OnRtRhTypeChanged(dataModelBinder_RTRH.RtRhMode);
                OnRtRhDataChangedAction(dataModelBinder_RTRH.RtRhData);
                dataModelBinder_RTRH.OnRtRhDataChangedAction += OnRtRhDataChangedAction;
                dataModelBinder_RTRH.OnRtRhTypeChangedAction += OnRtRhTypeChanged;
            }
        }
        private void OnDisable()
        {
            if (dataModelBinder_RTRH == null)
            {
                if (uiAnchorFollower.Target3DObject != null)
                {
                    uiAnchorFollower.Target3DObject.GetChild(0).TryGetComponent(out dataModelBinder_RTRH);

                }
                return;
            }
            else
            {
                dataModelBinder_RTRH.OnRtRhDataChangedAction -= OnRtRhDataChangedAction;
                dataModelBinder_RTRH.OnRtRhTypeChangedAction -= OnRtRhTypeChanged;
            }
        }
        #endregion
    }
}
