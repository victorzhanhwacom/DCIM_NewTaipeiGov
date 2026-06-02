using UnityEngine;

namespace VzDev.ApiExtensions
{
    public static class CoroutineExtension
    {
        /// [Extended] -  試著停止Coroutine
        public static void TryToStop(this Coroutine self, MonoBehaviour owner)
        {
            if (self == null) return;
            if (owner == null) return;
            owner.StopCoroutine(self);
        }
    }
}