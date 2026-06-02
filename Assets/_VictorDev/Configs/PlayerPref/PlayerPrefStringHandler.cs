using UnityEngine;

namespace VzDev.Configs
{
    public class PlayerPrefStringHandler : PlayerPrefParent<string>
    {
        public override void LoadValue(bool notify = true)
        {
            var value = PlayerPrefs.GetString(TagName, DefaultValue);
            if(notify) onValueLoaded?.Invoke(value);
        }
        
        public override void Receive(string value) => PlayerPrefs.SetString(TagName, value);
    }
}