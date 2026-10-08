using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using VzDev.DCIMUtils.DataUtils;
using VzDev.DCIMUtils.DeploymentUtils;
using VzDev.UnityAPI.Extensions;

public class RackResourceStatus : MonoBehaviour
{
    #region Event
    [Foldout("[Event]"), SerializeField] private UnityEvent<float> onTotalPowerRemainChangedEvent, onTotalWeightRemainChangedEvent, onTotalHeightURemainChangedEvent;
    [Foldout("[Event]"), SerializeField] private UnityEvent<float> onTotalPowerCapacityChangedEvent, onTotalWeightCapacityChangedEvent, onTotalHeightUCapacityChangedEvent;
    [Foldout("[Event]"), SerializeField] private UnityEvent<float> onTotalPowerRemainPercentChangedEvent, onTotalWeightRemainPercentChangedEvent, onTotalHeightURemainPercentChangedEvent;
    #endregion

    #region Fields
    [SerializeField, ReadOnly] private List<DataModelBinder_Rack> rackBinderList;
    [Foldout("[CalculateData]"), SerializeField, ReadOnly] private float totalPowerRemain, totalWeightRemain, totalHeightURemain;
    [Foldout("[CalculateData]"), SerializeField, ReadOnly] private float totalPowerCapacity, totalWeightCapacity, totalHeightUCapacity;
    [Foldout("[CalculateData]"), SerializeField, ReadOnly] private float totalPowerRemainPercent, totalWeightRemainPercent, totalHeightURemainPercent;
    #endregion

    #region Event Listener
    private void OnEnable()
    {
        RackDcrAssetSetter.OnRackDataCombinerGeneratedAction += HandleRackDataCombinerGenerated;
        HandleRackDataCombinerGenerated(RackDcrAssetSetter.RackDataModelBinders_Static);
    }
    private void OnDisable() => RackDcrAssetSetter.OnRackDataCombinerGeneratedAction -= HandleRackDataCombinerGenerated;
    private void HandleRackDataCombinerGenerated(List<DataModelBinder_Rack> list)
    {
        rackBinderList = list;
        CalculateTotalRackResources();
    }
    #endregion

    private void CalculateTotalRackResources()
    {
        ResetValue();
        //依照racks的UsageInfo，計算總電力使用、總重量使用、總高度U使用、總電力容量、總重量容量、總高度U容量、總電力使用百分比、總重量使用百分比、總高度U使用百分比
        foreach (var rackBinder in rackBinderList)
        {
            DCR_Asset rack = rackBinder.RackAsset;
            totalPowerRemain += rack.usageInfo.remainPowerWatt;
            totalWeightRemain += rack.usageInfo.remainWeightKG;
            totalHeightURemain += rack.usageInfo.remainHeightU;

            totalPowerCapacity += rack.power_watt_Max;
            totalWeightCapacity += rack.weight_kg_Max;
            totalHeightUCapacity += rack.u_height_Max;

            totalPowerRemainPercent += rack.usageInfo.remainPowerPercent;
            totalWeightRemainPercent += rack.usageInfo.remainWeightPercent;
            totalHeightURemainPercent += rack.usageInfo.remainHeightUPercent;
        }
        totalPowerRemainPercent = (totalPowerRemainPercent/rackBinderList.Count).RoundToDecimals(1);
        totalWeightRemainPercent = (totalWeightRemainPercent/rackBinderList.Count).RoundToDecimals(1);
        totalHeightURemainPercent = (totalHeightURemainPercent/rackBinderList.Count).RoundToDecimals(1);
        InvokeEvents();
    }

    private void InvokeEvents()
    {
        onTotalPowerRemainChangedEvent?.Invoke(totalPowerRemain);
        onTotalWeightRemainChangedEvent?.Invoke(totalWeightRemain);
        onTotalHeightURemainChangedEvent?.Invoke(totalHeightURemain);

        onTotalPowerCapacityChangedEvent?.Invoke(totalPowerCapacity);
        onTotalWeightCapacityChangedEvent?.Invoke(totalWeightCapacity);
        onTotalHeightUCapacityChangedEvent?.Invoke(totalHeightUCapacity);

        onTotalPowerRemainPercentChangedEvent?.Invoke(totalPowerRemainPercent);
        onTotalWeightRemainPercentChangedEvent?.Invoke(totalWeightRemainPercent);
        onTotalHeightURemainPercentChangedEvent?.Invoke(totalHeightURemainPercent);
    }

    private void ResetValue()
    {
        totalPowerRemain = 0f;
        totalWeightRemain = 0f;
        totalHeightURemain = 0f;

        totalPowerCapacity = 0f;
        totalWeightCapacity = 0f;
        totalHeightUCapacity = 0f;

        totalPowerRemainPercent = 0f;
        totalWeightRemainPercent = 0f;
        totalHeightURemainPercent = 0f;
    }
}
