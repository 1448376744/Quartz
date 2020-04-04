using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public interface IQuartzTaskFilter
    {
        /// <summary>
        /// 任务执行过滤
        /// </summary>
        /// <param name="context">任务执行上下文</param>
        /// <param name="next">任务委托</param>
        /// <returns></returns>
        Task OnExecuteAsync(TaskExecutingContext context, QuartzTaskExecuteDelegate next);
    }
    /// <summary>
    /// 任务执行委托
    /// </summary>
    /// <returns></returns>
    public delegate Task QuartzTaskExecuteDelegate();
}
