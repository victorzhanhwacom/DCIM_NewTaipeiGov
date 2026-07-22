using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using VzDev.UnityAPI.Extensions;

public class SetModelNameHandler : MonoBehaviour
{
    public List<GameObject> nameObjects;

    public string prefix = "TG+TPE+IDC+15F+RM_A+E+THS: THS+THS-15F-";

    [Button]
    private void UpdateName()
    {
        nameObjects.ForEach(obj =>
        {
            string index = obj.name.GetStringBetweenMarks("(", ")");
            obj.name += $"[{prefix}{(int.Parse(index)+1).ToString("D2")}]";
        });
    }
}
