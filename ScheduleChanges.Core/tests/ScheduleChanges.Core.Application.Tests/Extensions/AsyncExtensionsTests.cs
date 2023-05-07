using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ilya02Il.BaseTypes.Extensions.Tests
{
    public class AsyncExtensionsTests
    {
        [Fact]
        public void Task_Should_Cancel_Successfully()
        {
            var cancelTokenSrc = new CancellationTokenSource();
            var cancelToken = cancelTokenSrc.Token;

            var task = Task.Delay(10).WithCancellation(cancelToken);

            cancelTokenSrc.Cancel();

            Thread.Sleep(10);

            task.IsCanceled.Should().BeTrue();
        }

        [Fact]
        public void Generic_Task_Should_Cancel_Successfully()
        {
            var cancelTokenSrc = new CancellationTokenSource();
            var cancelToken = cancelTokenSrc.Token;

            var genericTask = Task.Run(async () =>
            {
                await Task.Delay(10);
                return 10;
            }).WithCancellation(cancelToken);

            cancelTokenSrc.Cancel();

            Thread.Sleep(10);

            genericTask.IsCanceled.Should().BeTrue();
        }

        [Fact]
        public async Task Non_Generic_Then_Should_Invoke_OnSuccess()
        {
            var task = Task.Delay(10);
            int result = 0;

            await task.Then(() => result = 1, () => result = 2);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Non_Generic_Then_Should_Invoke_OnCancelled()
        {
            var cancelTokenSrc = new CancellationTokenSource();
            var cancelToken = cancelTokenSrc.Token;

            var task = Task.Delay(10)
                .WithCancellation(cancelToken);
            int result = 0;

            cancelTokenSrc.Cancel();
            Thread.Sleep(10);

            await task.Then(() => result = 1, () => result = 2);

            result.Should().Be(2);
        }

        [Fact]
        public async Task Generic_Then_Should_Invoke_OnSuccess()
        {
            var task = Task.FromResult(1);

            var result = await task.Then(value => value, () => 2);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Generic_Then_Should_Invoke_OnCancelled()
        {
            var cancelTokenSrc = new CancellationTokenSource();
            var cancelToken = cancelTokenSrc.Token;

            var task = Task.Run(async () =>
            {
                await Task.Delay(10);
                return 10;
            }).WithCancellation(cancelToken);

            cancelTokenSrc.Cancel();
            Thread.Sleep(10);

            var result = await task.Then(value => value, () => 2);

            result.Should().Be(2);
        }

        [Fact]
        public async Task Non_Generic_Catch_Should_Handle_An_Exception()
        {
            const string message = "Test message";
            var task = Task.Run(() => throw new Exception(message));
            string result = string.Empty;

            await task.Catch(ex => result = ex.Message);

            result.Should().Be(message);
        }

        [Fact]
        public async Task Non_Generic_Catch_Should_Handle_An_Exception_From_Then()
        {
            var task = Task.Delay(10);

            await task.Then(() => throw new Exception())
                .Catch(ex => { });
        }

        [Fact]
        public async Task Generic_Catch_Should_Handle_An_Exception()
        {
            const string message = "Test message";
            var task = Task.Run(() => { throw new Exception(message); return 1; });

            var result = await task.Catch(ex => 2);

            result.Should().Be(2);
        }

        [Fact]
        public async Task Generic_Catch_Should_Handle_An_Exception_From_Then()
        {
            var task = Task.FromResult(10);

            await task.Then(() => throw new Exception())
                .Catch(ex => { });
        }

        [Fact]
        public async Task Non_Generic_Finally_Should_Invoke_FinallyBlock()
        {
            var task = Task.Delay(10);
            int result = 0;

            await task.Then(() => { })
                .Catch()
                .Finally(() => result = 1);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Generic_Finally_Should_Invoke_FinallyBlock()
        {
            var task = Task.FromResult(2);
            int result = 0;

            await task.Then(value => value)
                .Catch(exp => 1)
                .Finally(() => result = 1);

            result.Should().Be(1);

            // return _sender.Send(command)
            //      .Then(result => Ok(result))
            //      .Catch(exp => BadRequest(result));

            // return _sender.Send(command)
            //      .Catch(ex => new Result<SomeDto>(ex))
            //      .Then(result => result.Match(
            //          successResult => Ok(result.Value),
            //          faultedResult => BadRequest(result.Exception)
            //      ));
        }
    }
}
