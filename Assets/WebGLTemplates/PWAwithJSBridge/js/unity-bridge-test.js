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
      sendRandomPayloadToUnity();
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