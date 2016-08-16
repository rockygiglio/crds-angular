using crds_angular.Models.Crossroads;
using Crossroads.AsyncJobs.Interfaces;
using Quartz;
using Quartz.Impl;

namespace Crossroads.AsyncJobs.Processors
{
    public class ScheduledJobProcessor : IJobExecutor<ScheduledJob>
    {
        private readonly IScheduler _scheduler;
        public ScheduledJobProcessor(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Execute(Models.JobDetails<ScheduledJob> details)
        {
            ScheduledJob scheduledJob = details.Data;
            IJobDetail job = new JobDetailImpl("ScheduledJob", scheduledJob.JobType);
            job.JobDataMap.Put("dto", scheduledJob.Dto);

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(scheduledJob.StartDateTime)
                .Build();
            _scheduler.ScheduleJob(job, trigger);

        }
    }
}