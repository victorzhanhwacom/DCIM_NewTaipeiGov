using TMPro;
using UnityEngine;

namespace _VictorDev.ApiExtensions
{
    /// [Extended] TMP_Dropdown功能控制
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropDownValueEventNotify : MonoBehaviour
    {
        private TMP_Dropdown TmpDropdown => tmpDropdown ??= GetComponent<TMP_Dropdown>();
        private TMP_Dropdown tmpDropdown;

        public void SetValueWithNotify(int value)
        {
            if(TmpDropdown.value == value) TmpDropdown.onValueChanged.Invoke(value);
            else TmpDropdown.value = value;
        }
    }
}