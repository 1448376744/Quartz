using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace YC51.Quartz
{
    /// <summary>
    /// 扩展依赖注入
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQuartzFactory(this IServiceCollection services, Action<QuartzFactoryBuilder> builder)
        {
            services.AddSingleton(s =>
            {
                var qfb = new QuartzFactoryBuilder();
                qfb.PerExecuteUtcTime = () => DateTime.UtcNow.AddHours(8);
                builder(qfb);
                return new QuartzFactory(s, qfb);
            });
            return services;
        }
    }
}
