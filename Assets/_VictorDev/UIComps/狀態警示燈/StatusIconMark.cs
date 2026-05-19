using _VictorDev.Configs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _VictorDev.UIComps
{
    public class StatusIconMark : MonoBehaviour
    {
        #region Variables

        [SerializeField] private EnumIndicatorStatus indicatorStatus = EnumIndicatorStatus.Normal;
        [Foldout("[組件]"), SerializeField] private Image imgGood, imgWarning, imgOverload, imgMissingData;

        #endregion
        
        public void SetStatusGood() => SetStatus(EnumIndicatorStatus.Normal);
        public void SetStatusWarning() => SetStatus(EnumIndicatorStatus.Warning);
        public void SetStatusOverload() => SetStatus(EnumIndicatorStatus.Overload);
        public void SetStatusMissingData() => SetStatus(EnumIndicatorStatus.MissingData);

        private void SetStatus(EnumIndicatorStatus status)
        {
            indicatorStatus = status;
            UpdateUI();
        }

        private void UpdateUI()
        {
            imgGood.gameObject.SetActive(indicatorStatus == EnumIndicatorStatus.Normal);
            imgWarning.gameObject.SetActive(indicatorStatus == EnumIndicatorStatus.Warning);
            imgOverload.gameObject.SetActive(indicatorStatus == EnumIndicatorStatus.Overload);
            imgMissingData.gameObject.SetActive(indicatorStatus == EnumIndicatorStatus.MissingData);
        }

        [Button]
        private void FindComponents()
        {
            imgGood = transform.Find("iconGood").GetComponent<Image>();
            imgWarning = transform.Find("iconWarning").GetComponent<Image>();
            imgOverload = transform.Find("iconOverload").GetComponent<Image>();
            imgMissingData = transform.Find("iconMissingData").GetComponent<Image>();
        }

        private void OnValidate() => UpdateUI();

        private void Reset() => FindComponents();
    }
}