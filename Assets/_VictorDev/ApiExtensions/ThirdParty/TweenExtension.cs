using DG.Tweening;

namespace VzDev.ApiExtensions
{
    /// 原API類別功能擴充
    public static class TweenExtension
    {
        /// [Extended] -  試著刪除tween並清空為null值
        public static void TryToKill(this Tween self)
        {
            if (self == null) return;
            self.Kill();
        }
    }
}