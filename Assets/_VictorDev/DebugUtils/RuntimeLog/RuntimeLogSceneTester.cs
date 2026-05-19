using System;
using NaughtyAttributes;
using UnityEngine;

public class RuntimeLogSceneTester : MonoBehaviour
{
    [Button]
    private void LogError()
    {
        Debug.LogError("Error");
    }
    
    [Button]
    private void LogException()
    {
        throw new Exception("Exception");
    }
}
