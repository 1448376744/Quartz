using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    public class MyQuartzTask2 : IQuartzTask
    {
        public async Task ExecuteAsync(TaskExecutingContext context)
        {
           await Task.Run(()=> 
           {
               Console.WriteLine($"[2]Time:{DateTime.Now}Thread:{Thread.CurrentThread.ManagedThreadId},hello world");
           });
        }
    }
}
