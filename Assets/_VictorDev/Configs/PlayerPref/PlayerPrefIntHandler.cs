using System;
using UnityEngine;

namespace VzDev.Configs
{
    public class PlayerPrefParent : PlayerPrefParent<int>
    {
        public override void LoadValue(bool notify = true)
        {
            var value = PlayerPrefs.GetInt(TagName, DefaultValue);
            if(notify) onValueLoaded?.Invoke(value);
        }

        public override void Receive(int value) => PlayerPrefs.SetInt(TagName, value);
    }
}