using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VzDev.DCIMUtils;

public abstract class TagDetailPanel : MonoBehaviour
{
    #region Fields
    [SerializeField, ReadOnly] protected WebAPI_RealtimeData data;
    [Foldout("[Components]"), SerializeField] private ScrollRect scrollRect;
    [Foldout("[Components]"), SerializeField] protected GameObject rootView;
    [Foldout("[Components]"), SerializeField] private TextMeshProUGUI txtDeviceName;
    [Label("[Components]-ListItem"), SerializeField] private DetailListItem[] detailListItems;
    protected Toggle currentToggle;
    #endregion

    protected void SetData(WebAPI_RealtimeData realtimeData)
    {
        if (realtimeData == null)
        {
            Debug.LogWarning($"InRowCooler_DetailPanel: SetData called with null data.");
            return;
        }
        data = realtimeData;
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        txtDeviceName.SetText(data.deviceName);
        scrollRect.verticalNormalizedPosition = 1f; // 將ScrollRect的垂直位置設置為頂部

        string value;

        //將tags的displayName與value分別顯示在txtTagDisplayName與txtTagValue上
        for (int i = 0; i < data.tags.Count; i++)
        {
            DetailListItem detailListItem = detailListItems[i];
            if (detailListItem == null)
            {
                Debug.LogWarning($"TagDetailPanel: detailListItem at index {i} is null.\nDisplayName: {data.tags[i].displayName}, Value: {data.tags[i].value}, AlertLevelStatus: {data.tags[i].alertLevelStatus}");
                continue;
            }

            value = data.tags[i].value;
            if (data.tags[i].unit != null && data.tags[i].unit != "")
            {
                value += $" {data.tags[i].unit}";
            }

            detailListItem.SetData(data.tags[i].displayName, value, data.tags[i].alertLevelStatus);
        }
        rootView.SetActive(true); // 確保面板在更新後是可見的
    }

    public void ToHide()
    {
        if (currentToggle != null)
            currentToggle.isOn = false;
    }
}
