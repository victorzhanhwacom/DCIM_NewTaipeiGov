using System;
using System.Linq;
using UnityEngine;
using static VzDev.RealTimeDataDTO;

namespace VzDev
{
    [Serializable]
    public class RealtimeAsset_RtRh : RealtimeAsset
    {
        [field: SerializeField]
        public Tags rtTag { get; private set; }
        [field: SerializeField]
        public Tags rhTag { get; private set; }

        public override void SetTags(Tags[] tags)
        {
            base.SetTags(tags);
            rtTag = rawTags.FirstOrDefault(tag => tag.name == "dbt");
            rhTag = rawTags.FirstOrDefault(tag => tag.name == "rh");
        }
    }
}