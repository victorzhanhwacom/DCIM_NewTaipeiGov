using Debug = VzDev.ToolUtils.Debug;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace VzDev.FileUtils
{
    /// 檔案下載 For WebGL
    public static class WebGLFileDownloader
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DownloadFileFromBytes(
        byte[] data,
        int length,
        string fileName,
        string mimeType
    );
#endif

        public static void SaveExcelFile(byte[] bytes, string fileName = "ExcelFile.xlsx") => SaveFile(bytes, fileName,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        public static void SaveFile(
            byte[] bytes,
            string fileName,
            string mimeType = "application/octet-stream"
        )
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (bytes == null || bytes.Length == 0)
        {
            Debug.LogWarning("No data to download.");
            return;
        }

        DownloadFileFromBytes(bytes, bytes.Length, fileName, mimeType);
#else
            Debug.LogWarning("SaveFile is WebGL only.");
#endif
        }
    }
}