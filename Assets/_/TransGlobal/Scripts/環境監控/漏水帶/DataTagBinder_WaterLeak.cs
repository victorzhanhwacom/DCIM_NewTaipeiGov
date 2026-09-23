using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VzDev.ObjectUtils;
using static VzDev.DCIMUtils.EnviornmentUtils.RtRhDataManager;

namespace VzDev.DCIMUtils.EnviornmentUtils
{
    /// <summary>
    /// 漏水帶數據綁定器：標籤
    /// <para> 透過DataModelBinder_WaterLeak取得水帶模式 </para>
    /// </summary>
    public class DataTagBinder_WaterLeak : MonoBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private RealtimeAsset_WaterLeak waterLeakData;

        [Foldout("[Events]-Value")] public UnityEvent<string> onWaterLeakValueChangedEvent;
        [Foldout("[Events]-AlertLevel")] public UnityEvent<int> onAlertLevelChanged;
        [Foldout("[Components]"), SerializeField] private UIAnchorFollower uiAnchorFollower;
        [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName, txtCategory;
        private DataModelBinder_WaterLeak dataModelBinder_WaterLeak;
        #endregion

        private void Awake()
        {
            if (transform.TryGetComponent(out uiAnchorFollower) == false)
            {
                Debug.LogWarning($"DataTagBinder_WaterLeak: UIAnchorFollower component not found on the GameObject.", this);
            }
        }
        private void OnValidate() => Awake();

        /// <summary>
        /// 各項值的處理
        /// </summary>
        private void ValueHandler()
        {
            txtDeviceName?.SetText(waterLeakData.deviceName);
            txtCategory?.SetText(waterLeakData.category.ToString());
            onWaterLeakValueChangedEvent?.Invoke(waterLeakData.status);
            onAlertLevelChanged?.Invoke(waterLeakData.alertStatus);
        }

        /// <summary>
        /// 值改變時
        /// </summary>
        private void OnWaterLeakDataChangedAction(RealtimeAsset_WaterLeak data)
        {
            waterLeakData = data;
            ValueHandler();
        }


        #region Event Listeners
        private void OnEnable()
        {
            if (dataModelBinder_WaterLeak == null)
            {
                if (uiAnchorFollower.Target3DObject != null && uiAnchorFollower.Target3DObject.childCount > 0)
                {
                    uiAnchorFollower.Target3DObject.GetChild(0).TryGetComponent(out dataModelBinder_WaterLeak);
                }
                return;
            }
            else
            {
                OnWaterLeakDataChangedAction(dataModelBinder_WaterLeak.WaterLeakData);
                dataModelBinder_WaterLeak.OnWaterLeakDataChangedAction += OnWaterLeakDataChangedAction;
            }
        }
        private void OnDisable()
        {
            if (dataModelBinder_WaterLeak == null)
            {
                if (uiAnchorFollower.Target3DObject != null && uiAnchorFollower.Target3DObject.childCount > 0)
                {
                    uiAnchorFollower.Target3DObject.GetChild(0).TryGetComponent(out dataModelBinder_WaterLeak);
                }
                return;
            }
            else
            {
                dataModelBinder_WaterLeak.OnWaterLeakDataChangedAction -= OnWaterLeakDataChangedAction;
            }
        }
        #endregion
    }
}
