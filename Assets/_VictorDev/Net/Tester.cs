using VzDev.FileUtils;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public void ReceiveBytes(byte[] bytes)
    {
        WebGLFileDownloader.SaveExcelFile(bytes);
    }
}
