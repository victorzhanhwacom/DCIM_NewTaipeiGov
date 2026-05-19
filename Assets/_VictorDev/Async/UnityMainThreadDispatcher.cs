using System;
using System.Collections.Generic;
using _VictorDev.ApiExtensions;
using _VictorDev.DebugUtils;

namespace VictorDev.Async
{
    public class UnityMainThreadDispatcher : SingletonMonoBehaviour<UnityMainThreadDispatcher>
    {
        private static readonly Queue<Action> _actions = new Queue<Action>();

        public static void Enqueue(Action action)
        {
            if (action == null) return;

            lock (_actions)
            {
                _actions.Enqueue(action);
            }
        }

        private void Update()
        {
            lock (_actions)
            {
                while (_actions.Count > 0)
                {
                    var action = _actions.Dequeue();
                    action.Invoke();
                }
            }
        }
    }
}