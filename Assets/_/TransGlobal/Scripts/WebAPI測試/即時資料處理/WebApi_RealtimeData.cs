using System;
using System.Linq;
using UnityEngine;
using VzDev.DCIMUtils.DataUtils;
using static VzDev.WebApi_RealtimeDataDTO;

namespace VzDev
{
    /// <summary>
    /// For專案使用的資料格式(從WebAPI取得的即時資料轉換過來)
    /// </summary>
    public abstract class WebApi_RealtimeData : DCIMAsset
    {
        protected Tags[] rawTags;
        public virtual void SetTags(Tags[] tags) => rawTags = tags;
    }

    /// <summary>
    /// For只有單一項即時資料的裝置
    /// </summary>
    [Serializable]
    public class RealtimeAsset_SingleTag : WebApi_RealtimeData
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
