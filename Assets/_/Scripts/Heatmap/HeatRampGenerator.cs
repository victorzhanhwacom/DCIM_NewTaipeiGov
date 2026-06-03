using UnityEngine;

public static class HeatRampGenerator
{
    /// <summary>
    /// 依照溫度規格產生 HeatRamp Texture2D
    /// tempMin / tempMax 需與 HeatmapDataManager 一致
    /// </summary>
    public static Texture2D Create(
        float tempMin = 18f,
        float tempMax = 32f,
        int   width   = 256)
    {
        var tex = new Texture2D(width, 1, TextureFormat.RGBA32, false);
        tex.wrapMode   = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        // 將溫度轉為 0~1 的輔助函式
        float T(float celsius) => Mathf.Clamp01((celsius - tempMin) / (tempMax - tempMin));

        // ── 定義色階關鍵點（溫度 → 顏色）────────────────────
        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                // 18°C 以下：深藍
                new GradientColorKey(new Color(0.05f, 0.10f, 0.80f), T(18f)),
                // 20°C：天藍（過渡）
                new GradientColorKey(new Color(0.20f, 0.55f, 0.95f), T(20f)),
                // 22°C：綠色（正常）
                new GradientColorKey(new Color(0.15f, 0.80f, 0.25f), T(22f)),
                // 25°C：黃綠（微偏高）
                new GradientColorKey(new Color(0.70f, 0.90f, 0.10f), T(25f)),
                // 27°C：黃色（偏高）
                new GradientColorKey(new Color(1.00f, 0.85f, 0.00f), T(27f)),
                // 30°C：橙色（過熱）
                new GradientColorKey(new Color(1.00f, 0.45f, 0.00f), T(30f)),
                // 32°C 以上：紅色（危險）
                new GradientColorKey(new Color(0.90f, 0.05f, 0.05f), T(32f)),
            },
            new GradientAlphaKey[]
            {
                // 極低密度幾乎透明，高密度不透明
                // new GradientAlphaKey(0.00f, 0.00f),
                // new GradientAlphaKey(0.30f, 0.15f),
                new GradientAlphaKey(1.00f, 1.00f),
                new GradientAlphaKey(1.00f, 1.00f),
                new GradientAlphaKey(1.00f, 1.00f),
            }
        );

        // 採樣填入貼圖
        for (int i = 0; i < width; i++)
            tex.SetPixel(i, 0, gradient.Evaluate(i / (float)(width - 1)));

        tex.Apply();
        return tex;
    }
}