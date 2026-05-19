using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FollowGaugeText : MonoBehaviour
{
    [Header("References")]
    public Image fillImage;           // 環形進度條 Image
    public RectTransform textTransform; // 文字的 RectTransform
    public TextMeshProUGUI powerText;  // 文字組件

    [Header("Settings")]
    public float radius = 150f;       // 環形的半徑 (根據 UI 調整)
    [Range(0, 360)]
    public float startAngleOffset = 0f; // 起始角度偏移 (預設 Top 為 0)

    public void SetValue(float value) => UpdateGauge(value * 100, 100);
    
    // 複雜度最小化解法：直接計算座標
    // Minimum complexity solution: direct coordinate calculation
    public void UpdateGauge(float currentKw, float maxCapacity)
    {
        float ratio = Mathf.Clamp01(currentKw / maxCapacity);
        fillImage.fillAmount = ratio;
        powerText.text = $"{currentKw:F0}";

        // 1. 將 ratio 轉換為角度 (360度制)
        // Convert ratio to angles (360-degree system)
        float angle = ratio * 360f;

        // 2. 轉換為弧度並計算座標 (Unity UI 向上為 0 度，順時針旋轉)
        // Convert to radians and calculate coordinates (Up is 0 deg, Clockwise)
        float rad = (angle + startAngleOffset) * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad) * radius;
        float y = Mathf.Cos(rad) * radius;

        // 3. 更新文字位置
        // Update text position
        textTransform.localPosition = new Vector2(x, y);
    }
}