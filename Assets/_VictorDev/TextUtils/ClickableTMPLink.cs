using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _VictorDev.TextUtils
{
    /// 偵測TMP裡有link標籤的超鏈結，允許點擊並開啟URL
    public class ClickableTMPLink : MonoBehaviour, IPointerClickHandler
    {
        #region Variables

        [Foldout("[組件]"), SerializeField] private TextMeshProUGUI textMeshPro;
        [Foldout("[組件]"), SerializeField] private Canvas canvas;

        #endregion

        public void OnPointerClick(PointerEventData eventData)
        {
            Camera eventCamera = null;

            // 根據 Canvas 模式選擇正確相機
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                eventCamera = null;
            }
            else
            {
                eventCamera = canvas.worldCamera;
            }

            // 使用事件座標 eventData.position 而不是 Input.mousePosition（這是關鍵差別）
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, eventCamera);
            Debug.Log(linkIndex);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
                string linkId = linkInfo.GetLinkID();

                Debug.Log($"點擊到連結：{linkId}");
                Application.OpenURL(linkId);
            }
            else
            {
                Debug.Log("沒點到任何連結");
            }
        }

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            canvas = GetComponentInParent<Canvas>();
        }

        private void OnValidate() => Awake();
    }
}