using UnityEngine;
using UnityEngine.UI;
using TMPro; // 確保使用 TextMeshPro 提升科技質感

public class SimplePowerGauge : MonoBehaviour
{
    [Header("UI References")]
    public Image fillImage; // 圓環進度條
    public TextMeshProUGUI valueText; // 顯示數字

    [Header("Settings")]
    public float maxCapacity = 2500f; // 物理上限值

    // 複雜度最小化更新：直接賦值
    // Minimum complexity update: direct assignment
    public void SetPower(float currentKw)
    {
        // 計算填滿比例
        // Calculate fill ratio
        float ratio = currentKw / maxCapacity;
        fillImage.fillAmount = ratio;

        // 更新文字
        // Update text display
        valueText.text = $"{currentKw:F0}kW";

        // 簡單顏色切換：超過 0.8 (80%) 變紅
        // Simple color swap: turns red above 0.8
        fillImage.color = ratio > 0.8f ? Color.red : Color.cyan;
    }
}