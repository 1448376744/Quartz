using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YC51.Quartz;

namespace ConsoleTesting
{
    public class MyQuartzTaskFilter2 : IQuartzTaskFilter
    {
        readonly ILogger<MyQuartzTaskFilter2> _logger;
        public MyQuartzTaskFilter2(ILogger<MyQuartzTaskFilter2> logger)
        {
            _logger = logger;
        }
        public async Task OnExecuteAsync(TaskExecutingContext context, QuartzTaskExecuteDelegate next)
        {
            _logger.LogDebug("[过滤器2][线程ID:{Thread.CurrentThread.ManagedThreadId}] 开启事务");
            await next();
            _logger.LogDebug("[过滤器2][线程ID:{Thread.CurrentThread.ManagedThreadId}] 关闭事务");
        }
    }
}
