using TMPro;

namespace VzDev.ApiExtensions
{
    /// 原API TextMeshProExtension類別功能擴充
    public static class TextMeshProExtension
    {
        /// [Extended] - 將游標移至最後面
        public static TMP_InputField MoveCaretToEnd(this TMP_InputField self)
        {
            // 把 caretPosition 和 selectionAnchor 都設到最後
            int endPos = self.text.Length;
            self.caretPosition = endPos;
            self.selectionAnchorPosition = endPos;
            return self;
        }
    }
}