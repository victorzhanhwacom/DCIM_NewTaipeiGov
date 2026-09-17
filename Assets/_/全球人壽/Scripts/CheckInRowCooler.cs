using System.Collections.Generic;
using UnityEngine;

public class CheckInRowCooler : MonoBehaviour
{
    public void LogModelList(List<Transform> models)
    {
        if (models == null || models.Count == 0)
        {
            Debug.Log("Model list is empty.");
            return;
        }

        string modelNames = $"Model List ({models.Count}):\n";
        foreach (var model in models)
        {
            if (model != null)
            {
                modelNames += $"- {model.name}\n";
            }
        }

        Debug.Log(modelNames);
    }
}
