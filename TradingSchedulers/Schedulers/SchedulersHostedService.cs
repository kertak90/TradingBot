using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingSchedulers.Schedulers.Jobs;

namespace TradingSchedulers.Schedulers
{
    class SchedulersHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SchedulersHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = (IJobFactory)_serviceProvider.GetService(typeof(IJobFactory));
            await scheduler.Start();
            var job = (IJob)_serviceProvider.GetService(typeof(IJob));
            
            IJobDetail jobDetail = JobBuilder.Create<ChartIntersectionAnalyzeJob>()
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("IntersectionAnalyzeTrigger")
                .StartNow()
                .WithCronSchedule("0/5 * * * * ?")                
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
