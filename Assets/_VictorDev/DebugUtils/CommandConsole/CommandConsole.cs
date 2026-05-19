using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using _VictorDev.ApiExtensions;

namespace _VictorDev.ApiExtensions
{
    /// 命令控制台
    public class CommandConsole : MonoBehaviour
    {
        #region Variables

        [Label("指令設定(LCtrl+`呼叫命令控制台)"), SerializeField] private List<CommandSet> commandList;
        [Foldout("[組件]"), SerializeField] private GameObject uiObject;
        [Foldout("[組件]"), SerializeField] private TMP_InputField txtCommandHistory;
        [Foldout("[組件]"), SerializeField] private TMP_InputField inputCommandLine;

        private List<string> _commandHistoryList = new List<string>();
        private int _commandHistoryIndex = 0;
        
        private string _txtCommandRecord = "";
        private bool _isOn = false;

        #endregion

        /// 判別指令碼
        public void SubmitCommand(string command)
        {
            command = command.Trim();
            if (string.IsNullOrEmpty(command) == false)
            {
                CommandSet target = commandList.FirstOrDefault(commandSet =>
                    commandSet.command.Equals(command, StringComparison.OrdinalIgnoreCase));

                string dateTime = DateTime.Now.ToString("HH:mm:ss");
                
                string newCommandLine = string.IsNullOrWhiteSpace(_txtCommandRecord) ? "" : "\n"; 
                newCommandLine += $"<b>[{dateTime}]</b> {command}";
                if (target == null){newCommandLine += "\t<color=#ff0000>-> The command is NOT exist!</color>";}
                
                _txtCommandRecord += newCommandLine;
                txtCommandHistory.text = _txtCommandRecord;
                txtCommandHistory.verticalScrollbar.value = 1;
                target?.callBack?.Invoke();
                
                _commandHistoryList.Add(command);
                _commandHistoryIndex = _commandHistoryList.Count;

            }
            inputCommandLine.text = string.Empty;
            inputCommandLine.ActivateInputField();
        }

        /// 清除歷史記錄
        public void ClearCommandHistory()
        {
            _txtCommandRecord = string.Empty;
            txtCommandHistory.text = string.Empty;
            txtCommandHistory.verticalScrollbar.value = 1;
            inputCommandLine.text = string.Empty;
            inputCommandLine.ActivateInputField();
        }

        /// 關閉命令列
        public void CloseCommandConsole()
        {
            SetIsOn(false);   
            int index = _txtCommandRecord.LastIndexOf('\n');
            if (index >= 0)
            {
                _txtCommandRecord = _txtCommandRecord.Substring(0, index);
                txtCommandHistory.text = _txtCommandRecord;
                txtCommandHistory.verticalScrollbar.value = 1;
            }
        }

        /// 設定是否顯示
        private void SetIsOn(bool isOn)
        {
            _isOn = isOn;
            uiObject.SetActive(_isOn);
            if (_isOn)  inputCommandLine.ActivateInputField();
        }
        
        /// 操作方式以開啟/關閉CommandConsole
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.BackQuote))
            {
                SetIsOn(!_isOn);
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)) ReadCommandHistory(-1);
            if(Input.GetKeyDown(KeyCode.DownArrow)) ReadCommandHistory(1);
        }

        /// 讀取之前輸入過的Command
        private void ReadCommandHistory(int jumpIndex)
        {
            if(_commandHistoryList.Count == 0) return;
            _commandHistoryIndex = Mathf.Clamp(_commandHistoryIndex + jumpIndex, 0, _commandHistoryList.Count - 1);
            inputCommandLine.text = _commandHistoryList[_commandHistoryIndex];
            inputCommandLine.MoveCaretToEnd();
        }


        private void Awake() => uiObject.SetActive(_isOn);

        private void OnEnable() => inputCommandLine.onSubmit.AddListener(SubmitCommand);
        private void OnDisable() => inputCommandLine.onSubmit.RemoveListener(SubmitCommand);
    }

    [Serializable]
    public class CommandSet
    {
        public string command;
        public UnityEvent callBack;
    }
}