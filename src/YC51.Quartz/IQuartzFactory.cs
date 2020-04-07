using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    public interface IQuartzFactory
    {
        Task StartAsync();
        void Stop();
    }
}
