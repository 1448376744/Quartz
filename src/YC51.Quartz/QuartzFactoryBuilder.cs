using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace YC51.Quartz
{
    /// <summary>
    /// 任务工厂构建
    /// </summary>
    public class QuartzFactoryBuilder
    {
        /// <summary>
        /// 设置每次,默认为:DateTime.UtcNow.AddHours(8)
        /// </summary>
        public Func<DateTime> PerExecuteUtcTime { get; set; }

        internal List<QuartzTaskBuilder> Tasks { get; }
            = new List<QuartzTaskBuilder>();

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <typeparam name="TTask">任务类型</typeparam>
        /// <param name="expression">cron表达式，只支持6个域</param>
        public void AddTask<TTask>(string expression)
            where TTask : IQuartzTask
        {
            var cron = Cronos.CronExpression.Parse(expression, Cronos.CronFormat.IncludeSeconds);
            Tasks.Add(new QuartzTaskBuilder(typeof(TTask), cron));
        }

        /// <summary>
        /// 添加一个任务和过滤器
        /// </summary>
        /// <typeparam name="TTask">任务类型</typeparam>
        /// <param name="expression">cron表达式，只支持6个域</param>
        /// <param name="filterTypes">过滤器组</param>
        public void AddTask<TTask>(string expression, params Type[] filterTypes)
          where TTask : IQuartzTask
        {
            var cron = Cronos.CronExpression.Parse(expression, Cronos.CronFormat.IncludeSeconds);
            Tasks.Add(new QuartzTaskBuilder(typeof(TTask), cron, filterTypes));
        }
    }
}
