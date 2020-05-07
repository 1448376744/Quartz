# Quartz

## 完全基于.net core容器的任务调度，支持过滤器在每次执行任务时执行过滤，每次执行任务都会创建一个新的实例（Scope）以及过滤器实例，来执行。  内置基于静态代理实现的AOP

``` C#
static async Task Main(string[] args)
{
    //创建容器
    var services = new ServiceCollection();
    services.AddQuartzFactory(b =>
    {
        //设置时区
        b.PerExecuteUtcTime = () => DateTime.UtcNow.AddHours(8);
        //添加任务
        b.AddTask<MyQuartzTask>("0/1 * * * * ?", typeof(MyQuartzTaskFilter), typeof(MyQuartzTaskFilter2));
    });
    //构建容器
    var provider = services.BuildServiceProvider();
    //从容器获取任务调度工厂
    var factory = provider.GetRequiredService<QuartzFactory>();
    //启动任务调度
    await factory.StartAsync();
}

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

public class MyQuartzTaskFilter : IQuartzTaskFilter
{
    public async Task OnExecuteAsync(TaskExecutingContext context, QuartzTaskExecuteDelegate next)
    {           
        Console.WriteLine("[过滤器1][线程ID:{Thread.CurrentThread.ManagedThreadId}] 开启事务");
        await next();
        Console.WriteLine("[过滤器1][线程ID:{Thread.CurrentThread.ManagedThreadId}] 关闭事务");
    }
}


```
