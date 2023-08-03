using System.Runtime.CompilerServices;
using System;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.Networking;
using UnityEngine;

namespace AtomicAssetsApiClient
{
    /// <summary>
    /// Class to implement async / awayt on a UnityWebRequest class
    /// </summary>
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation asyncOp;
        private Action continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted { get { return asyncOp.isDone; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            if (continuation != null)
                continuation();
        }
    }

    /// <summary>
    /// Extender to augment UnityWebRequest clas
    /// </summary>
    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}
#endif