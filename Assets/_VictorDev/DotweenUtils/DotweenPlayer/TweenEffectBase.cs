using NaughtyAttributes;
using UnityEngine;

namespace _VictorDev.DebugUtils
{
    public class TweenEffectBase : MonoBehaviour
    {
        [Label("[Base Property]"), SerializeField] protected TweenBaseProperty tweenBaseProperty;
    }

    public enum TweenMethod
    {
        TweenTo,
        TweenFrom
    }
    
    public enum TweenValueType
    {
        Absolute,
        Relative
    }
}