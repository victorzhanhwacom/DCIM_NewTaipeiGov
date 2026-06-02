using UnityEngine;

namespace VzDev.Configs
{
    public class PlayerPrefBoolHandler : PlayerPrefParent<bool>
    {
        public override void LoadValue(bool notify = true)
        {
            var value = PlayerPrefs.GetInt(TagName, DefaultValue? 1 : 0);
            if(notify) onValueLoaded?.Invoke(value == 1);
        }
        public override void Receive(bool value) => PlayerPrefs.SetInt(TagName, value ? 1 : 0);
    }
}