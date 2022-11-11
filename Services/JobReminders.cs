using partner_aluro.Data;
using partner_aluro.Models;
using Quartz;

namespace partner_aluro.Services
{
    public class JobReminders : IJob
    {
        public JobReminders()
        {
        }

        public Task Execute(IJobExecutionContext context)
        {
            Common.Logs($"JobReminders at " + DateTime.Now.ToString("dd-mm-yyyy hh:mm:ss"), "JobReminders" + DateTime.Now.ToString("hhmmss"));
            return Task.CompletedTask;
        }
    }
}
