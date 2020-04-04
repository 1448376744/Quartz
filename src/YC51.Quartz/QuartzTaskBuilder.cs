using Cronos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    /// <summary>
    /// 任务构建
    /// </summary>
    internal class QuartzTaskBuilder
    {
        /// <summary>
        /// 构建任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="expression">cron表达式</param>
        public QuartzTaskBuilder(Type taskType, CronExpression expression)
        {
            TaskType = taskType;
            Expression = expression;
        }
        /// <summary>
        /// 构建任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="expression">cron表达式</param>
        /// <param name="filterTypes">过滤器组</param>
        public QuartzTaskBuilder(Type taskType, CronExpression expression, params Type[] filterTypes)
        {
            TaskType = taskType;
            FilterTypes = filterTypes;
            Expression = expression;
        }
        /// <summary>
        /// Cron表达式
        /// </summary>
        internal CronExpression Expression { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        internal Type TaskType { get; private set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        internal Type[] FilterTypes { get; private set; } = Type.EmptyTypes;
    }
}
