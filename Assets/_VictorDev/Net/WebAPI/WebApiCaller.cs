using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using _VictorDev.ApiExtensions;
using _VictorDev.FileUtils;
using Unity.VisualScripting;
using UnityEngine;
using VictorDev.Managers;
using Debug = _VictorDev.DebugUtils.Debug;

namespace _VictorDev.Net.WebAPI
{
    public static class WebApiCaller
    {
        /// Get 請求
        public static void GetAsync(string url, Action<string> onSuccess, Action<string> onFailed = null)
        {
            WebApiRequestSO data = ScriptableObject.CreateInstance<WebApiRequestSO>();
            data.SetUrl(url);
            SendRequest(data, onSuccess, onFailed);
        }
        /// Post 請求
        public static void PostAsync(string url, string bodyJson, Action<string> onSuccess, Action<string> onFailed = null)
        {
            WebApiRequestSO data = ScriptableObject.CreateInstance<WebApiRequestSO>();
            data.SetUrl(url, EnumHttpMethod.POST);
            data.SetBodyRawJson(bodyJson);
            SendRequest(data, onSuccess, onFailed);
        }

        /// 統一處理呼叫WebAPI (WebApiRequest類別)
        public static void SendRequest(WebApiRequestSO apiRequestSo, Action<string> onSuccess, Action<string> onFailed = null)
        {
            if (apiRequestSo == null)
            {
                Debug.LogWarning($"WebApiRequest is null", typeof(This), EmojiEnum.Warning);
                return;
            }

            onFailed ??= DefaultOnFailed;

            Debug.Log($"{apiRequestSo.name}: [{apiRequestSo.EnumHttpMethod}] {apiRequestSo.URL}", nameof(WebApiRequestSO),
                EmojiEnum.Upload);

            TaskManager.Run($"SendRequest_{apiRequestSo.name}", RunTask);
            
            // 執行Task：呼叫WebAPI
            async Task RunTask(CancellationToken token)
            {
                
                if (apiRequestSo.EnumHttpMethod == EnumHttpMethod.PATCH)
                {
                    string result = await SendPatchByUnityWebRequest(apiRequestSo, token);
                    Debug.Log($"SendRequest Success (PATCH)\n{result}",
                        nameof(WebApiRequestSO), EmojiEnum.Success);
                    onSuccess?.Invoke(result);
                    return;
                }
                
                try
                {
                    token.ThrowIfCancellationRequested(); // 支援取消
                    HttpResponseMessage response = await Client.SendAsync(apiRequestSo.HttpRequestMessage, token);
                    response.EnsureSuccessStatusCode(); // 如果這個 HTTP 回應不是成功的狀態碼（200～299），就 直接丟出例外，跳到 catch 區塊

                    string responseContent = string.Empty;
                    // 依回傳資料型態進行資料處理
                    switch (FileHelper.GetResponseDataTypeFromHttpHeader(response.Content))
                    {
                        case EnumResponseDataType.Json: // Json字串
                            responseContent = await response.Content.ReadAsStringAsync();
                            responseContent = responseContent.ToJsonFormat();
                            break;
                        case EnumResponseDataType.Excel:
                            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                            string fileName = FileHelper.GetFileNameFromHttpHeader(response.Content);
                            //responseContent  = await FileHelper.SaveFileWithPopupWindow(fileBytes, fileName);
                            break;
                        case EnumResponseDataType.Text:
                            responseContent = await response.Content.ReadAsStringAsync();
                            break;
                    }
                    
                    Debug.Log($"SendRequest Success: [{(int)response.StatusCode}]\n{responseContent}",nameof(WebApiRequestSO), EmojiEnum.Success);
                    onSuccess?.Invoke(responseContent);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"SendRequest Exception: {ex.Message}", nameof(WebApiCallHandler), EmojiEnum.Warning);
                    onFailed?.Invoke(ex.Message);
                }
            }
        }
        
        /// [Patch]方式的另外處理
        private static async Task<string> SendPatchByUnityWebRequest(
            WebApiRequestSO apiRequest,
            CancellationToken token)
        {
            using var request = new UnityEngine.Networking.UnityWebRequest(
                apiRequest.URL,
                "PATCH");

            // ===== Body =====
            if (!string.IsNullOrEmpty(apiRequest.BodyRawJson))
            {
                byte[] bodyBytes =
                    System.Text.Encoding.UTF8.GetBytes(apiRequest.BodyRawJson);

                request.uploadHandler =
                    new UnityEngine.Networking.UploadHandlerRaw(bodyBytes);

                request.SetRequestHeader("Content-Type", apiRequest.MediaType);
            }

            request.downloadHandler =
                new UnityEngine.Networking.DownloadHandlerBuffer();

            request.SetRequestHeader("Accept", "application/json");
            // ===== 關鍵：複製 Header（含 Authorization）=====
            if (apiRequest.HttpRequestMessage != null)
            {
                foreach (var header in apiRequest.HttpRequestMessage.Headers)
                {
                    foreach (var value in header.Value)
                    {
                        request.SetRequestHeader(header.Key, value);
                    }
                }
            }

            // ===== Send =====
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                if (token.IsCancellationRequested)
                {
                    request.Abort();
                    throw new OperationCanceledException();
                }
                await Task.Yield();
            }

            if (request.result !=
                UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                throw new Exception(request.error);
            }

            return request.downloadHandler.text;
        }

      

        /// 預設的OnFail事件處理
        private static void DefaultOnFailed(string msg) => Debug.LogWarning($"DefaultOnFailed! \n{msg}", nameof(WebApiCallHandler), EmojiEnum.Warning);
        private static readonly HttpClient Client = new();
    }
}