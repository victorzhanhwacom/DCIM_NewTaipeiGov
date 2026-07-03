/**
 * Unity WebGL <-> JavaScript 橋接工具 - 核心模組
 * 負責:Unity instance 註冊、JS -> Unity 送出邏輯、未就緒訊息暫存。
 * 此檔案獨立於 index.html 產物,位於 Assets/WebGLTemplates/ 下受版控管理,
 * 不會因 Unity 重新 Build 而被覆蓋。
 *
 * 接收 Unity 端訊息的函式(C# -> JS)定義在 unity-bridge-handlers.js,
 * 此檔案只負責「框架」本身,不包含任何業務邏輯。
 */

// 先確保 window.UnityBridge 存在,避免載入順序造成 handlers 檔案的內容被覆蓋
window.UnityBridge = window.UnityBridge || {};

window.unityReady = false;
window.unityInstance = null;

var _pendingMessages = [];

/**
 * Unity 初始化完成後,由 index.html 的 .then() 呼叫此函式註冊 instance。
 */
function registerUnityInstance(instance) {
  window.unityInstance = instance;
  window.unityReady = true;
  console.log("[UnityBridge] Unity 初始化完成");

  flushPendingMessages();
}

function flushPendingMessages() {
  if (_pendingMessages.length === 0) return;
  var queued = _pendingMessages;
  _pendingMessages = [];
  queued.forEach(function (msg) {
    sendToUnityByCustom(msg.gameObjectName, msg.methodName, msg.payload);
  });
}

// 送出訊息給 Unity端 ===============================
const UnityObjName = "WebGLBridge";
const UnityMethodName = "OnReceiveFromJS";

/**
 * 送出訊息給預設的 Unity 物件/方法 (UnityObjName / UnityMethodName)。
 * @param {object|string} payload 任意物件(會自動 JSON.stringify)或字串
 */
function sendToUnity(payload) {
  sendToUnityByCustom(UnityObjName, UnityMethodName, payload);
}

/**
 * 送出訊息給指定的 Unity 物件/方法。
 * @param {string} gameObjectName 場景上接收訊息的 GameObject 名稱
 * @param {string} methodName 該物件上要呼叫的 public 方法
 * @param {object|string} payload 任意物件(會自動 JSON.stringify)或字串
 */
function sendToUnityByCustom(gameObjectName, methodName, payload) {
  if (typeof gameObjectName !== "string" || typeof methodName !== "string") {
    console.error("[UnityBridge] gameObjectName / methodName 必須是字串,實際收到:", gameObjectName, methodName);
    return;
  }

  if (!window.unityReady || !window.unityInstance) {
    console.warn("[UnityBridge] Unity 尚未就緒,訊息已暫存,待就緒後補送");
    _pendingMessages.push({ gameObjectName, methodName, payload });
    return;
  }

  var data = typeof payload === "string" ? payload : JSON.stringify(payload);

  try {
    window.unityInstance.SendMessage(gameObjectName, methodName, data);
    console.log("[UnityBridge] SendMessage -> " + gameObjectName + "." + methodName, data);
  } catch (e) {
    console.error("[UnityBridge] SendMessage 失敗:", e);
  }
}
// 送出訊息給 Unity端 ===============================