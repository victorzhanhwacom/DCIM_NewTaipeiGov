using System.Linq;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using static VzDev.RealTimeDataDTO;

namespace VzDev
{
    /// <summary>
    /// For專案使用的資料格式(從WebAPI取得的即時資料轉換過來)
    /// </summary>
    public abstract class RealtimeAsset : DCIMAsset
    {
        protected Tags[] rawTags;
        public virtual void SetTags(Tags[] tags) => rawTags = tags;
    }

    /// <summary>
    /// For只有單一項即時資料的裝置
    /// </summary>
    public abstract class RealtimeAsset_Single : RealtimeAsset
    {
        [field: SerializeField]
        public Tags tag { get; private set; }
        public override void SetTags(Tags[] tags)
        {
            base.SetTags(tags);
            tag = rawTags.FirstOrDefault();
        }
    }
}
