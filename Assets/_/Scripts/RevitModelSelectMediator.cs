using UnityEngine;
using UnityEngine.Events;
using VzDev.DCIM.Deployment;
using VzDev.UnityAPI.Extensions;

public class RevitModelSelectMediator : MonoBehaviour
{
    public UnityEvent<Transform, DCR_Asset> OnUpsSelected;
    public UnityEvent<Transform, DCR_Asset> OnRtRhSelected;
    public UnityEvent<Transform, DCR_Asset> OnLeakSelected;
    public UnityEvent<Transform, DCR_Asset> OnRackSelected;

    private Transform lastSelectedModel;

    public void SetSelectedModel(Transform model)
    {
        lastSelectedModel = model;
        string deviceCode = lastSelectedModel.name.GetStringBetweenMarks("[", "]");

        GetDataFromWebAPI(deviceCode);
      
    }


    public void GetDataFromWebAPI(string deviceCode)
    {
        // Call the web API to get data based on the device code
        // This is a placeholder for the actual implementation
        Debug.Log($"Fetching data for device code: {deviceCode}");
        CheckTypeToInvoke(deviceCode);
        
    }

    private void CheckTypeToInvoke(string deviceCode)
    {
        if(deviceCode.Contains("UPS")) OnUpsSelected?.Invoke(lastSelectedModel, new DCR_Asset { assetInfo = new AssetInfo { deviceCode = deviceCode } });
        else if(deviceCode.Contains("THS")) OnRtRhSelected?.Invoke(lastSelectedModel, new DCR_Asset { assetInfo = new AssetInfo { deviceCode = deviceCode } });
        else if(deviceCode.Contains("Leak")) OnLeakSelected?.Invoke(lastSelectedModel, new DCR_Asset { assetInfo = new AssetInfo { deviceCode = deviceCode } });
        else if(deviceCode.Contains("DCR")) OnRackSelected?.Invoke(lastSelectedModel, new DCR_Asset { assetInfo = new AssetInfo { deviceCode = deviceCode } });
    }

    public void DelectedModel(Transform model)
    {
    }
}
