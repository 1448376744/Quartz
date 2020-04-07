using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace YC51.Quartz
{
    /// <summary>
    /// 任务调度工厂
    /// </summary>
    public class QuartzFactory : IQuartzFactory
    {
        internal IServiceProvider _provider;

        internal QuartzFactoryBuilder _builder;

        private System.Timers.Timer _timer;

        private TaskCompletionSource<int> _completionSource;

        internal QuartzFactory(IServiceProvider provider, QuartzFactoryBuilder builder)
        {
            _builder = builder;
            _provider = provider;
            _completionSource = new TaskCompletionSource<int>();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            var time0 = new DateTime(1970);
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Stop();
                }
                //此刻
                var time1 = _builder.PerExecuteUtcTime();
                //迭代所有任务
                foreach (var item in _builder.Tasks)
                {
                    //计算当前的任务下次执行时间
                    var time2 = item.Expression.GetNextOccurrence(time1, true).Value;
                    //只计算秒数部分
                    var timestamp1 = unchecked((long)(time1 - time0).TotalSeconds);
                    var timestamp2 = unchecked((long)(time2 - time0).TotalSeconds);
                    if (timestamp1 == timestamp2)
                    {
                        QueueUserWorkItem(item);
                    }
                }
            };
            _timer.Enabled = true;
            _timer.Start();
            await _completionSource.Task;
        }

        public void Stop()
        {
            _completionSource.SetResult(0);
            _timer.Stop();
        }

        /// <summary>
        /// 使用线程池排队执行任务
        /// </summary>
        /// <param name="builder"></param>
        private void QueueUserWorkItem(QuartzTaskBuilder builder)
        {
            //使用线程池调度
            ThreadPool.QueueUserWorkItem(async (state) =>
            {
                using (var scope = _provider.CreateScope())
                {
                    var task = ActivatorUtilities
                           .GetServiceOrCreateInstance(scope.ServiceProvider, builder.TaskType) as IQuartzTask;
                    var filters = new List<IQuartzTaskFilter>();
                    foreach (var item in builder.FilterTypes)
                    {
                        var filter = ActivatorUtilities
                                .GetServiceOrCreateInstance(scope.ServiceProvider, item) as IQuartzTaskFilter;
                        filters.Add(filter);
                    }
                    var executor = new QuartzFilterExecutor(filters);
                    await executor.Execute(() => task.ExecuteAsync(new TaskExecutingContext
                    {

                    }));
                }
            });
        }
    }
}
