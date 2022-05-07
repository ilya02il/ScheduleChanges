using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;

        public PerformanceBehavior()
        {
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            Console.WriteLine("Elapsed milliseconds:" + _timer.Elapsed.ToString());

            return response;
        }
    }
}
