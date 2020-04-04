using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    /// <summary>
    /// 任务
    /// </summary>
    public interface IQuartzTask
    {
        Task ExecuteAsync(TaskExecutingContext context);
    }
}
