using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NotifierDaemon
{
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Task executingTask;
        private readonly CancellationTokenSource stoppingCts = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            executingTask = ExecuteAsync(stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (executingTask.IsCompleted)
            {
                return executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            stoppingCts.Cancel();
        }
    }

    public class NotifierDaemonService : BackgroundService
    {
        private readonly ICriticalEventsConsumer consumer;

        public NotifierDaemonService(ICriticalEventsConsumer consumer)
        {
            this.consumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => consumer.ProcessEvents(stoppingToken));
        }
    }
}
