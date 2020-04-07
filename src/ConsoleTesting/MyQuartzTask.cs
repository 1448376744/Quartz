using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    public class MyQuartzTask : IQuartzTask
    {
        readonly ILogger<MyQuartzTask> _logger;
        public MyQuartzTask(ILogger<MyQuartzTask> logger)
        {
            _logger = logger;
        }
        public async Task ExecuteAsync(TaskExecutingContext context)
        {
            await Task.Run(() =>
            {
                _logger.LogError($"[任务：1][时间:{DateTime.Now}][线程ID:{Thread.CurrentThread.ManagedThreadId}],执行目标任务");
            });
        }
    }
}
