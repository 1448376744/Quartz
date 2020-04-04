using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YC51.Quartz;

namespace ConsoleTesting
{
    public class MyQuartzTaskFilter : IQuartzTaskFilter
    {
        public async Task OnExecuteAsync(TaskExecutingContext context, QuartzTaskExecuteDelegate next)
        {           
            Console.WriteLine("[过滤器1][线程ID:{Thread.CurrentThread.ManagedThreadId}] 开启事务");
            await next();
            Console.WriteLine("[过滤器1][线程ID:{Thread.CurrentThread.ManagedThreadId}] 关闭事务");
        }
    }
}
