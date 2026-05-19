using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DynamicGradientController : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Gradient gradient; // 在 Inspector 直接調顏色、數量與比例
    [SerializeField, Range(0, 1), HideInInspector] private float progress = 0f; //不知有何作用

    private Texture2D _tempTexture;
    private static readonly int GradTexHash = Shader.PropertyToID("_GradientTex");
    private static readonly int ProgressHash = Shader.PropertyToID("_T");

    void Update()
    {
        //TryToGetImageMaterial();
        if (targetMaterial == null || gradient == null) return;

        UpdateGradientTexture();
        targetMaterial.SetFloat(ProgressHash, progress);
    }

    private void TryToGetImageMaterial()
    {
        if (targetMaterial == null && TryGetComponent(out Image image))
        {
            targetMaterial = image.material;
        }
    }

    private void UpdateGradientTexture()
    {
        // 只有在材質或漸層改變時更新，維持 O(1) 的渲染效能
        if (_tempTexture == null)
        {
            _tempTexture = new Texture2D(256, 1, TextureFormat.RGBA32, false);
            _tempTexture.wrapMode = TextureWrapMode.Clamp;
            _tempTexture.filterMode = FilterMode.Bilinear;
        }

        for (int i = 0; i < 256; i++)
        {
            float t = i / 255f;
            _tempTexture.SetPixel(i, 0, gradient.Evaluate(t));
        }
        _tempTexture.Apply();
        targetMaterial.SetTexture(GradTexHash, _tempTexture);
    }

    private void OnDestroy()
    {
        if (_tempTexture != null) DestroyImmediate(_tempTexture);
    }
}