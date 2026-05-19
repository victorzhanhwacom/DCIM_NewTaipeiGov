using System.Collections;
using UnityEngine;

namespace _VictorDev.Managers
{
    /// Coroutine分區段
    public static class CoroutineChunkRunner
    {
        /// 由呼叫端啟動 Chunk 協程
        public static Coroutine Run(MonoBehaviour caller, IEnumerator routine, int chunkSize = 300)
        {
            return caller.StartCoroutine(RunChunked(routine, chunkSize));
        }

        private static IEnumerator RunChunked(IEnumerator routine, int chunkSize)
        {
            int counter = 0;
            while (routine.MoveNext())
            {
                counter++;
                // 大家都靠這個來避免 freeze
                if (counter >= chunkSize)
                {
                    counter = 0;
                    yield return null;
                }
                yield return routine.Current;
            }
        }
    }

}