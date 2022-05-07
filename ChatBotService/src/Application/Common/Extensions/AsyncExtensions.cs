using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Extensions
{
    public static class AsyncExtensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(state =>
            {
                ((TaskCompletionSource<object>)state).TrySetResult(null);
            },
            tcs))
            {
                var resultTask = await Task.WhenAny(task, tcs.Task);

                if (resultTask == tcs.Task)
                    throw new OperationCanceledException(cancellationToken);

                return await task;
            }
        }
    }
}
