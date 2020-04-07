using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YC51.Quartz;

namespace ConsoleTesting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //创建容器
            var services = new ServiceCollection();
            services.AddLogging(c=> 
            {
                c.AddNLog();
            });
            services.AddQuartzFactory(b =>
            {
                //设置时区
                b.PerExecuteUtcTime = () => DateTime.UtcNow.AddHours(8);
                //添加任务
                b.AddTask<MyQuartzTask>("0/1 * * * * ?", typeof(MyQuartzTaskFilter), typeof(MyQuartzTaskFilter2));
            });
            //构建容器
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<ILogger<Program>>().LogInformation("fff");
            //从容器获取任务调度工厂
            var factory = provider.GetRequiredService<IQuartzFactory>();
            //启动任务调度
            var cancel = new CancellationTokenSource();
            factory.StartAsync(cancel.Token);
            Thread.Sleep(5000);
            cancel.Cancel();
        }
    }
}
