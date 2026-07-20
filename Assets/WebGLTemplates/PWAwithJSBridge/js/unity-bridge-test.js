/**
 * 測試用按鈕:動態建立 UI,不需修改 index.html。
 * 正式環境不需要時,直接移除 index.html 裡對這支檔案的 <script> 引用即可。
 */
const ENABLE_TEST_BUTTON = true;

function sendRandomPayloadToUnity() {
  sendToUnity({
    type: "randomTest",
    value: Math.floor(Math.random() * 1000),
    ratio: Number(Math.random().toFixed(3)),
    timestamp: Date.now()
  });
}

// 傳送使用者 Token 給 Unity
function sendUserTokenToUnity(){
  userToken = "Loing User Token: " + Math.floor(Math.random() * 1000);
  sendToUnityByCustom("WebGLBridge_User", "SetUserToken", userToken);
}
// 傳送主選單索引給 Unity
function sendMainMenuToUnity() {
  mainMenuIndex = Math.floor(Math.random() * 7).toString(); // 隨機生成 0~6 的字串
  sendToUnityByCustom("WebGLBridge_MainMenu", "SetMainMenu", mainMenuIndex);
}

// 傳送環控子選單索引給 Unity
function sendEnvSubMenuToUnity() {
  subMenuIndex = Math.floor(Math.random() * 3).toString(); // 隨機生成 0、1 或 2 的字串
  sendToUnityByCustom("WebGLBridge_Env", "SetSubMenu", subMenuIndex);
}

function createTestButton() {
  var btn = document.createElement("button");
  btn.id = "test-send-btn";
  btn.textContent = "JS Send Data to Unity";

  btn.style.position = "fixed";
  btn.style.top = "10px";
  btn.style.right = "10px";
  btn.style.zIndex = "9999";
  btn.style.padding = "8px 16px";
  btn.style.fontSize = "16px";
  btn.style.cursor = "pointer";
  btn.style.border = "1px solid #444";
  btn.style.borderRadius = "6px";
  btn.style.background = "#1e1e1e";
  btn.style.color = "#fff";

  btn.addEventListener("click", function (e) {
    try {
      //sendRandomPayloadToUnity();
      //sendUserTokenToUnity();
      sendMainMenuToUnity();
      //sendEnvSubMenuToUnity();
    } catch (err) {
      console.error("[UnityBridge] click handler 發生例外:", err);
    }
  });

  document.body.appendChild(btn);
}

document.addEventListener("DOMContentLoaded", function () {
  if (ENABLE_TEST_BUTTON) {
    createTestButton();
  }
});