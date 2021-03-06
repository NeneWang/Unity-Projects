// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using UniRx.Async;

namespace Naninovel
{
    public static class AsyncUtils
    {
        public static YieldAwaitable WaitEndOfFrame => UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

        public static UniTask.Awaiter GetAwaiter (this UniTask? task) => task?.GetAwaiter() ?? UniTask.CompletedTask.GetAwaiter();

        public static UniTask<T>.Awaiter GetAwaiter<T> (this UniTask<T>? task) => task?.GetAwaiter() ?? UniTask.FromResult<T>(default).GetAwaiter();
    }
}
