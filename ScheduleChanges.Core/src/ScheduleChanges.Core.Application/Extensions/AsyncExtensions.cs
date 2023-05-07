namespace Ilya02Il.BaseTypes.Extensions;

/// <summary>
/// Методы расширения для <see cref="Task"/> и <see cref="Task{TResult}"/>
/// </summary>
public static class AsyncExtensions
{
    /// <summary>
    ///     Метод расширения для <see cref="Task{TResult}"/>,
    ///     позволяющий добавить <see cref="Task{TResult}"/> в
    ///     <see cref="TaskCompletionSource{TResult}"/>,
    ///     токеном которого является <paramref name="cancelToken"/>
    /// </summary>
    /// <typeparam name="T">
    ///     Возвращаемый задачей тип
    /// </typeparam>
    public static async Task<T> WithCancellation<T>(
        this Task<T> task,
        CancellationToken cancelToken)
    {
        return await (Task<T>)(await TaskCancellation<T>(task, cancelToken));
    }

    /// <summary>
    ///     Метод расширения для <see cref="Task"/>,
    ///     позволяющий добавить <see cref="Task"/> в
    ///     <see cref="TaskCompletionSource{TResult}"/>,
    ///     токеном которого является <paramref name="cancelToken"/>
    /// </summary>
    public static async Task WithCancellation(
        this Task task,
        CancellationToken cancelToken)
    {
        await TaskCancellation<object>(task, cancelToken);
    }

    /// <summary>
    ///     Метод расширения, принимающий как аргументы делегаты, которые выполняются
    ///     сразу после выполнения <paramref name="task"/> в зависимости от ее статуса.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="onSuccess">
    ///     Функция, которая вызывается сразу после того,
    ///     как <paramref name="task"/> была успешно выполнена
    /// </param>
    /// <param name="onCancelled">
    ///     Функция, которая вызывается сразу после того,
    ///     как <paramref name="task"/> была отменена
    /// </param>
    public static async Task Then(
        this Task task,
        Action onSuccess,
        Action? onCancelled = null)
    {
        try
        {
            await task;
            onSuccess();
        }
        catch (OperationCanceledException)
        {
            if (onCancelled == null)
            {
                await Task.CompletedTask;
                return;
            }

            onCancelled();
            return;
        }
        catch
        {
            throw;
        }
    }

    /// <inheritdoc cref="Then(Task, Action, Action)"/>
    /// <typeparam name="TIn">
    ///     Тип, возвращаемый <paramref name="task"/>.
    /// </typeparam>
    /// <typeparam name="TOut">
    ///     Тип, возвращаемый функциями <paramref name="onSuccess"/> и <paramref name="onCancelled"/>.
    /// </typeparam>
    public static async Task<TOut> Then<TIn, TOut>(
        this Task<TIn> task,
        Func<TIn, TOut> onSuccess,
        Func<TOut>? onCancelled = null)
    {
        try
        {
            var taskResult = await task;
            return onSuccess(taskResult);
        }
        catch (OperationCanceledException)
        {
            if (onCancelled == null)
                throw new ArgumentNullException(nameof(onCancelled));

            return onCancelled();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///     Метод расширения, принимающий как аргумент делегат <paramref name="catchBlock"/>,
    ///     который выполняется если во время выполнения <paramref name="task"/>
    ///     было выкинуто исключение.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="catchBlock">
    ///     Функция, которая выполняется если во время выполнения <paramref name="task"/> было выброшено исключение.
    /// </param>
    public static async Task Catch(this Task task, Action<Exception>? catchBlock = null)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            catchBlock?.Invoke(ex);
        }
    }

    /// <inheritdoc cref="Catch(Task, Action{Exception})"/>
    /// <typeparam name="TOut">
    ///     Тип, возвращаемый <paramref name="task"/> и <paramref name="catchBlock"/>.
    /// </typeparam>
    public static async Task<TOut> Catch<TOut>(
        this Task<TOut> task,
        Func<Exception, TOut> catchBlock)
    {
        try
        {
            return await task;
        }
        catch (Exception ex)
        {
            return catchBlock(ex);
        }
    }

    /// <summary>
    ///     Метод расширения, принимающий как аргумент делегат, который выполняется после завершения <paramref name="task"/>.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="finallyBlock">
    ///     Функция, которая выполняется после выполнения <paramref name="task"/>
    /// </param>
    public static async Task Finally(this Task task, Action finallyBlock)
    {
        try
        {
            await task;
        }
        finally
        {
            finallyBlock();
        }
    }

    /// <inheritdoc cref="Finally(Task, Action)"/>
    /// <typeparam name="TOut">
    /// Тип, возвращаемый <paramref name="task"/>.
    /// </typeparam>
    public static async Task<TOut> Finally<TOut>(this Task<TOut> task,
        Action finallyBlock)
    {
        try
        {
            return await task;
        }
        finally
        {
            finallyBlock();
        }
    }

    private static async Task<Task> TaskCancellation<T>(Task task, CancellationToken cancelToken)
    {
        var taskCompletionSrc =
            new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

        var cancelTokenRegistration = cancelToken.Register(state =>
        {
            var taskCompletionSource = state as TaskCompletionSource<T>;

            taskCompletionSource?.TrySetResult(default);
        }, taskCompletionSrc);

        using (cancelTokenRegistration)
        {
            var resultTask = await Task.WhenAny(task, taskCompletionSrc.Task);

            if (resultTask == taskCompletionSrc.Task)
                throw new OperationCanceledException(cancelToken);

            return task;
        }
    }
}
