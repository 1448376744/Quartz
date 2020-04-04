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
        public async Task ExecuteAsync(TaskExecutingContext context)
        {
            await Task.Run(() =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[任务：1][时间:{DateTime.Now}][线程ID:{Thread.CurrentThread.ManagedThreadId}],执行目标任务");
                Console.ForegroundColor = ConsoleColor.White;
            });
        }
    }
}
