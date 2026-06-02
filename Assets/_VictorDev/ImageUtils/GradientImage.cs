using VzDev.ApiExtensions;
using VzDev.FileUtils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VzDev.ImageUtils
{
    /// 產生漸層圖像，並Invoke Texture2D/Sprite
    public class GradientImage : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Gradient gradient;
        [Foldout("[Event] Invoke漸層Texture2D")] public UnityEvent<Texture2D> invokeTextureEvent;
        [Foldout("[Event] Invoke漸層Sprite")] public UnityEvent<Sprite> invokeSpriteEvent;
        [Foldout("[組件(選填)]"), SerializeField] private Image imgTarget;
        [Foldout("[設定]"), SerializeField] private bool saveToFile = false;
        [Foldout("[設定]"), SerializeField, ShowIf(nameof(saveToFile))] private string exportFileName = "MyImage";
        [Foldout("[設定]"), SerializeField] private int width = 256;
        [Foldout("[設定]"), SerializeField] private int height = 1; // UI 用橫向漸層通常 1 就行

        #endregion

        [Button]
        private async void CreateGradientImage()
        {
            Texture2D texture2D = gradient.CreateTexture2D(width, height);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, width, height), Vector2.zero);
            invokeTextureEvent?.Invoke(texture2D);
            invokeSpriteEvent?.Invoke(sprite);
            
            if (imgTarget != null) imgTarget.sprite = sprite;

            if (saveToFile)
            {
                string filePath = await FileHelper.SaveTexturePNGAsync(texture2D, exportFileName);
                Debug.Log($"file saved: {filePath}");
            }
        }

        private void OnValidate()
        {
            if(imgTarget == null) imgTarget = GetComponent<Image>();
        }
    }
}