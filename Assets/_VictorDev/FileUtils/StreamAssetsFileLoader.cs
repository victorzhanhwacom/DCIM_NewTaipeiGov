using System;
using System.Collections;
using System.IO;
using System.Text;
using _VictorDev.DebugUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Debug = _VictorDev.DebugUtils.Debug;

namespace _VictorDev.FileUtils
{
    ///  StreamAssets資料夾裡的檔案讀取
    ///  <para> + 資料是未打包的，適用於影片、JSON、文字檔</para>
    public class StreamAssetsFileLoader : SingletonMonoBehaviour<StreamAssetsFileLoader>
    {
        /// 檢查StreamAssets資料夾是否存在，若無則自動新增
        public static void CheckStreamingAssetsFolder()
        {
            string parentFolder = "Assets", newFolderName = "StreamingAssets";
            string fullPath = Path.Combine(parentFolder, newFolderName);

#if UNITY_EDITOR
            // 檢查是否已存在
            if (!AssetDatabase.IsValidFolder(fullPath))
            {
                // 建立新資料夾
                AssetDatabase.CreateFolder(parentFolder, newFolderName);
                UnityEngine.Debug.Log("資料夾建立成功: " + fullPath);
            }
#endif

        }

        /// 讀取JSON
        public static Coroutine LoadJsonFile(string filePath, Action<string> onSuccess, Action onFailed = null)
        {
            //if(filePath.EndsWith(".json") == false) filePath += ".json";
            filePath = filePath.Trim();
            if (Path.HasExtension(filePath) == false)
            {
                Debug.LogError($"{filePath} is not a valid file path.");
                return null;
            }
            return LoadTextFile(filePath, onSuccess, onFailed);
        }

        /// 讀取文字檔
        public static Coroutine LoadTextFile(string filePath, Action<string> onSuccess, Action onFailed = null)
        {
            CheckStreamingAssetsFolder();
            //if(filePath.EndsWith(".json") == false && filePath.EndsWith(".txt") == false) filePath += ".txt";
            if (Application.isEditor)
            {
                LoadTextFileInEditor(filePath, onSuccess, onFailed);
                return null;
            }
            return Instance.StartCoroutine(LoadTextFileAsync());
            IEnumerator LoadTextFileAsync()
            {
                filePath = Path.Combine(Application.streamingAssetsPath, filePath).Replace("\\", "/");
                Debug.Log($"讀取StreamAssets文字檔: {filePath}", Instance, EmojiEnum.Download);

                string result = "";
                if (Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // Android 需要用 UnityWebRequest 讀取 StreamingAssets 內的檔案
                    using UnityWebRequest request = UnityWebRequest.Get(filePath);
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                        result = request.downloadHandler.text;
                    else
                    {
                        onFailed?.Invoke();
                        UnityEngine.Debug.LogError($"讀取失敗: {request.error}");
                        yield break;
                    }
                }
                else
                {
                    // 其他平台 (Windows, Mac, iOS) 用 StreamReader 逐步讀取 JSON
                    if (File.Exists(filePath))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            while (reader.ReadLine() is { } line)
                            {
                                stringBuilder.Append(line);
                                yield return null; // 讓 Unity 在每次讀取一行後，回到主線程，避免卡頓
                            }
                        }
                        result = stringBuilder.ToString();
                    }
                    else
                    {
                        onFailed?.Invoke();
                        yield break;
                    }
                }
                UnityEngine.Debug.Log("JSON 讀取完成");
                onSuccess?.Invoke(result);
            }
        }
       
        /// [Editor] 讀取JSON檔案
        public static void LoadJsonFileInEditor(string filePath, Action<string> onSuccess, Action onFailed = null)
        {
            if(filePath.EndsWith(".json") == false) filePath += ".json";
            LoadTextFileInEditor(filePath, onSuccess, onFailed);
        }

        /// [Editor] 讀取文字檔案
        public static void LoadTextFileInEditor(string filePath, Action<string> onSuccess, Action onFailed = null)
        {
            
            //if (filePath.EndsWith(".json") == false && filePath.EndsWith(".txt") == false) filePath += ".txt";
            if (CheckFileExistsInEditor(ref filePath))
            {
                var data = File.ReadAllText(filePath);
                onSuccess?.Invoke(data);
            }
            else onFailed?.Invoke();
        }
        
        /// 判斷檔案是否存在
        private static bool CheckFileExistsInEditor(ref string filePath)
        {
            filePath = "Assets/StreamingAssets/" + filePath;
            var isExists = File.Exists(filePath);
            if (isExists == false) UnityEngine.Debug.LogError($"檔案不存在: {filePath}");
            return isExists;
        }
    }
}  