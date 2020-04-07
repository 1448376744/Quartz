using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YC51.Quartz;

namespace ConsoleTesting
{
    public class MyQuartzTaskFilter : IQuartzTaskFilter
    {
        readonly ILogger<MyQuartzTaskFilter> _logger;
        public MyQuartzTaskFilter(ILogger<MyQuartzTaskFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnExecuteAsync(TaskExecutingContext context, QuartzTaskExecuteDelegate next)
        {
            _logger.LogDebug("[过滤器1][线程ID:{Thread.CurrentThread.ManagedThreadId}] 开启事务");
            await next();
            _logger.LogDebug("[过滤器1][线程ID:{Thread.CurrentThread.ManagedThreadId}] 关闭事务");
        }
    }
}
