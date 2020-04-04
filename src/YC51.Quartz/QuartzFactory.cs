using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace YC51.Quartz
{
    /// <summary>
    /// 任务调度工厂
    /// </summary>
    public class QuartzFactory
    {
        internal IServiceProvider _provider;

        internal QuartzFactoryBuilder _builder;

        internal QuartzFactory(IServiceProvider provider, QuartzFactoryBuilder builder)
        {
            _builder = builder;
            _provider = provider;
        }

        public async Task StartAsync()
        {
            var time0 = new DateTime(1970);
            await Task.Run(() =>
            {
                while (true)
                {
                    //此刻
                    var time1 = _builder.PerExecuteUtcTime();
                    //迭代所有任务
                    foreach (var item in _builder.Tasks)
                    {
                        //计算当前的任务下次执行时间
                        var time2 = item.Expression.GetNextOccurrence(time1, true).Value;
                        //只计算秒数部分
                        var timestamp1 = (time1 - time0).TotalSeconds;
                        var timestamp2 = (time2 - time0).TotalSeconds;
                        if (timestamp1 == timestamp2)
                        {
                            QueueUserWorkItem(item);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 使用线程池排队执行任务
        /// </summary>
        /// <param name="builder"></param>
        private void QueueUserWorkItem(QuartzTaskBuilder builder)
        {
            using (var scope = _provider.CreateScope())
            {
                //使用线程池调度
                ThreadPool.QueueUserWorkItem(async (state) =>
                {
                    var task = ActivatorUtilities
                       .GetServiceOrCreateInstance(_provider, builder.TaskType) as IQuartzTask;
                    var filters = new List<IQuartzTaskFilter>();
                    foreach (var item in builder.FilterTypes)
                    {
                        var filter = ActivatorUtilities
                            .GetServiceOrCreateInstance(_provider, item) as IQuartzTaskFilter;
                        filters.Add(filter);
                    }
                    var executor = new QuartzFilterExecutor(filters);
                    await executor.Execute(() => task.ExecuteAsync(new TaskExecutingContext
                    {

                    }));
                });
            }
        }
    }
}
