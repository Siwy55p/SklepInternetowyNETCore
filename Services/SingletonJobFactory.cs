using partner_aluro.Data;
using partner_aluro.Models;
using Quartz;
using Quartz.Spi;

namespace partner_aluro.Services
{
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;  

        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
        //    Common.Logs($"NewJob at " + DateTime.Now.ToString("dd-mm-yyyy hh:mm:ss"), "NewJob" + DateTime.Now.ToString("hhmmss"));

            return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {

        }

    }
}
