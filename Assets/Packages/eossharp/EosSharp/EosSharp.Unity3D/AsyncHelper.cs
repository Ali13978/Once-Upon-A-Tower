using System;
using System.Threading.Tasks;
using UnityEngine;

namespace eossharp.EosSharp.EosSharp.Unity3D
{
    public static class AsyncHelper
    {
        public static async Task Delay(int durationMiliseconds)
        {
            var endTime = DateTimeOffset.Now.AddMilliseconds(durationMiliseconds);

#if UNITY_WEBGL && !UNITY_EDITOR_WIN
            while(DateTimeOffset.Now < endTime)
            {
                await Task.Yield();
            }
#else
            await Task.Delay(durationMiliseconds);

#endif
        }
    }
}
