using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using VzDev.Frameworks.LifecycleUtils;

public class ResizeByText : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    public Vector2 preferredSize;

    private string lastText = "";
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        GlobalLifecycleBroadcaster.OnGlobalUpdate += OnUpdate;
    }

    private void OnUpdate()
    {
        if (textMeshProUGUI != null && textMeshProUGUI.text != lastText)
        {
            lastText = textMeshProUGUI.text;
            Resize();
        }
    }

    [Button]
    private void Resize()
    {
        // 取得文字的首選尺寸
        preferredSize = textMeshProUGUI.GetPreferredValues();

        // 設定 RectTransform 的大小
        rectTransform.sizeDelta = new Vector2(preferredSize.x, rectTransform.sizeDelta.y);
    }
}
