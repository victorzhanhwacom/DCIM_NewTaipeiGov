using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Debug = _VictorDev.DebugUtils.Debug;

namespace _VictorDev.FileUtils
{
    /// [Editor] - 讀取StreamingAssets資料夾裡的機櫃Json檔
    public class TextFileLoader : MonoBehaviour
    {
        /// 建立StreamingAssets資料夾
        [Button]
        private void CreateStreamingAssetsFolder()
        {
            Debug.Log("CreateStreamingAssetsFolder...", this, EmojiEnum.Folder);
            StreamAssetsFileLoader.CheckStreamingAssetsFolder();
            Debug.Log("CreateStreamingAssetsFolder... OK!", this, EmojiEnum.Done);
        }

        /// 讀取Json檔案
        [Button]
        public void LoadTextFile() => LoadTextFile(streamAssetfilePath);
        public void LoadTextFile(string path)
        {
            jsonString = string.Empty;
            if (string.IsNullOrEmpty(streamAssetfilePath)) streamAssetfilePath = path;
            streamAssetfilePath = streamAssetfilePath.Trim();
            Debug.Log("LoadJsonFile...", this, EmojiEnum.Download);
            StreamAssetsFileLoader.LoadJsonFile(streamAssetfilePath, OnSuccessHandler);
        }

        private void OnSuccessHandler(string data)
        {
            jsonString = data;
            Debug.Log($"LoadJsonFile... OK!\n{jsonString}", this, EmojiEnum.Done);
            InvokeJsonString();
        }

        public void InvokeJsonString() => invokeOnSuccess?.Invoke(jsonString);

        private void Start()
        {
            if (isReloadOnStart) LoadTextFile();
            else if (isInvokeOnStart) invokeOnSuccess?.Invoke(jsonString);
        }

        #region Variables

        [Foldout("[Event] - 成功時Invoke")] public UnityEvent<string> invokeOnSuccess;

        [Foldout("[設定]")] [SerializeField] string streamAssetfilePath = "jsonfile";
        [Foldout("[設定]")] [SerializeField] bool isReloadOnStart = false;
        [Foldout("[設定]")] [SerializeField] bool isInvokeOnStart = true;

        ///序列化，但不顯示在Inspector上
        [Foldout("[資料項]")] [SerializeField, HideInInspector]
        private string jsonString;

        #endregion
    }
}