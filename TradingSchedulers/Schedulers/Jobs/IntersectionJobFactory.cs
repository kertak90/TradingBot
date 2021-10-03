using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSchedulers.Schedulers.Jobs
{
    public class IntersectionJobFactory : IJobFactory
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private IJob _job;

        public IntersectionJobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;            
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var job = scope.ServiceProvider.GetService(typeof(ChartIntersectionAnalyzeJob)) as IJob;
                return job;
            }
        }

        public void ReturnJob(IJob job)
        {
            //Do something if need

        }
    }
}
