using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    public interface IQuartzFactory : IDisposable
    {
        Task StartAsync(CancellationToken cancellationToken = default);
        void Stop();
    }
}
