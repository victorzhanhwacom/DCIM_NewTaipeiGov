using System;
using System.IO;
using System.Threading.Tasks;
using _VictorDev.FileUtils;
using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.ApiExtensions
{
    /// LOG檔案管理
    public class LogFileManager : MonoBehaviour
    {
        #region Variables

        [SerializeField, ReadOnly, TextArea(0, 2), Label("LOG檔路徑")]
        private string fullFilePath;

        [Foldout("[設定]"), SerializeField] private string fileName = "RuntimeLog";
        [Foldout("[設定]"), SerializeField] private bool isSaveByDate = true;

        [Foldout("[設定]"), SerializeField, Label("保存天數(0=不刪舊檔)"), ShowIf(nameof(isSaveByDate))]
        private int daysToKeep = 7; // 保留最近幾天的 log，設 0 表示不刪舊

        private DateTime? _currentLogDate; //目前日期
        private string FolderPath => Application.persistentDataPath; // LOG檔案資料夾路徑

        private bool _isDeletingLogs;

        #endregion

        /// 將Log訊息寫入文字檔
        public void WriteMessageToLogFile(string logMessage)
        {
            logMessage = logMessage.TrimEnd(' ');

            if (IsSameLogDate() == false)
            {
                // 刪舊檔（在跨日的時候也順便做）
                if (daysToKeep > 0) DeleteOldLogsAsync(daysToKeep);
            }

            RefreshFullFilePath();
            FileHelper.WriteStringToTextFile(fullFilePath, logMessage);
        }

        #region Private Funs

        private void Awake()
        {
            fileName = fileName.Trim();
            IsSameLogDate();
            // 刪舊（如果有設定）
            if (daysToKeep > 0) DeleteOldLogsAsync(daysToKeep);
        }

        // 更新路徑與日期
        private bool IsSameLogDate()
        {
            DateTime today = DateTime.Today;
            bool isSameDate = _currentLogDate == today;
            if (isSameDate == false) _currentLogDate = today;
            return isSameDate;
        }

        /// 重新整理LOG檔名與路徑
        private void RefreshFullFilePath()
        {
            string dateStr = "";
            if (isSaveByDate)
            {
                _currentLogDate ??= DateTime.Today;
                dateStr = $"_{_currentLogDate:yyyy-M-d dddd}";
            }

            fullFilePath = Path.Combine(FolderPath, $"{fileName}{dateStr}.txt");
        }

        private void OnValidate() => RefreshFullFilePath();

        [Button]
        private void OpenFolderPath() => FileHelper.OpenFileOrFolder(FolderPath);

        [Button]
        private void OpenLogFile() => FileHelper.OpenFileOrFolder(fullFilePath);

        // 刪除過舊 log（保留最近 keepDays 的檔案）

        private async void DeleteOldLogsAsync(int fileKeepDays)
        {
            if (_isDeletingLogs) return;
            _isDeletingLogs = true;

            try
            {
                await Task.Run(() =>
                {
                    var dir = new DirectoryInfo(Application.persistentDataPath);
                    var files = dir.GetFiles($"{fileName}_*.txt");
                    var cutoff = DateTime.Now.Date.AddDays(-fileKeepDays + 1);

                    foreach (var f in files)
                    {
                        string nameWithoutExt  = Path.GetFileNameWithoutExtension(f.Name);
                        int idx = nameWithoutExt .IndexOf('_');
                        if (idx >= 0)
                        {
                            string datePart = nameWithoutExt .Substring(idx + 1).Split(' ')[0];
                            if (DateTime.TryParse(datePart, out DateTime fileDate))
                            {
                                if (fileDate.Date < cutoff)
                                {
                                    try
                                    {
                                        f.Delete();
                                    }
                                    catch
                                    {
                                        /* 忽略刪除失敗 */
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogWarning($"DeleteOldLogsAsync 失敗: {e.Message}");
            }
            finally
            {
                _isDeletingLogs = false;
            }
        }
        #endregion
    }
}