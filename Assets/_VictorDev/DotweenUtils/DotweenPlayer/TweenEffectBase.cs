using NaughtyAttributes;
using UnityEngine;

namespace VzDev.DebugUtils
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