using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YC51.Quartz
{
    /// <summary>
    /// 过滤器执行器
    /// </summary>
    internal class QuartzFilterExecutor
    {
        /// <summary>
        /// 过滤器
        /// </summary>
        private readonly Queue<IQuartzTaskFilter> _filters = new Queue<IQuartzTaskFilter>();

        /// <summary>
        /// 创建一个过滤器执行器
        /// </summary>
        /// <param name="filters"></param>
        public QuartzFilterExecutor(List<IQuartzTaskFilter> filters)
        {
            foreach (var item in filters)
            {
                _filters.Enqueue(item);
            }
        }

        /// <summary>
        /// 执行过滤器栈
        /// </summary>
        /// <param name="next">任务执行器</param>
        /// <returns></returns>
        public Task Execute(Func<Task> next)
        {
            return Builder(next)();
        }

        /// <summary>
        /// 构建执行器栈
        /// </summary>
        /// <param name="next">任务执行器</param>
        /// <returns></returns>
        private Func<Task> Builder(Func<Task> next)
        {
            if (_filters.Count > 0)
            {
                var filter = _filters.Dequeue();
                async Task current()
                {
                    await filter.OnExecuteAsync(new TaskExecutingContext
                    {

                    }, new QuartzTaskExecuteDelegate(Builder(next)));
                }
                return current;
            }
            else
            {
                return next;
            }
        }
    }
}
