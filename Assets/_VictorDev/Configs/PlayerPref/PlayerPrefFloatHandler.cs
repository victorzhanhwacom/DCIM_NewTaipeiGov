using UnityEngine;

namespace VzDev.Configs
{
    public class PlayerPrefFloatHandler : PlayerPrefParent<float>
    {
        public override void LoadValue(bool notify = true)
        {
            var value = PlayerPrefs.GetFloat(TagName, DefaultValue);
            if(notify) onValueLoaded?.Invoke(value);
        }
        public override void Receive(float value) => PlayerPrefs.SetFloat(TagName, value);
    }
}