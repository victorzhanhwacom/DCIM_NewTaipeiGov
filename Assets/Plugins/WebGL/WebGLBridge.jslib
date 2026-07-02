mergeInto(LibraryManager.library, {

  // C# -> JS:單純呼叫,傳字串參數(數字/物件請自行 JSON.stringify 後傳入)
  VzDev_SendToJS: function (functionNamePtr, payloadPtr) {
    var functionName = UTF8ToString(functionNamePtr);
    var payload = UTF8ToString(payloadPtr);

    // 外部網頁可覆寫 window.VzDevBridge[functionName] 來接收
    if (window.VzDevBridge && typeof window.VzDevBridge[functionName] === "function") {
      window.VzDevBridge[functionName](payload);
    } else {
      console.warn("[VzDevBridge] 找不到對應的 JS 處理函式: " + functionName, payload);
    }
  },

  // 讓 JS 主動詢問 Unity 是否已就緒(可選)
  VzDev_IsUnityReady: function () {
    return window.__vzdevUnityReady === true ? 1 : 0;
  }

});