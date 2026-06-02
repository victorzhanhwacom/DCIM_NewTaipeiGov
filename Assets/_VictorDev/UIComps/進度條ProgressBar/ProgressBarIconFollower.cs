using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace VzDev.UIComps.ProgressBarUtils
{
    [RequireComponent(typeof(Image))]
    public class ProgressBarIconFollower : MonoBehaviour
    {
        #region Varaibles

        [Foldout("[組件]"), SerializeField] private Image fillImage; // 做 fillAmount 的那個 Image
        [SerializeField] private RectTransform icon; // 要跟著跑的 Icon
        [SerializeField] private RectTransform fillArea; // Fill 的實際範圍（避免算錯）

        #endregion

        void Update()
        {
            float rectWidth = fillImage.rectTransform.rect.width;
            icon.anchoredPosition = new Vector2(fillImage.fillAmount * rectWidth, icon.anchoredPosition.y);
        }

        private void OnValidate()
        {
            fillImage = GetComponent<Image>();
            fillImage.type = Image.Type.Filled;
        }
    }
}