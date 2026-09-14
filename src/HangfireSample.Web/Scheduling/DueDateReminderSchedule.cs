using Hangfire;
using Hangfire.Common;
using HangfireSample.Web.Jobs;

namespace HangfireSample.Web.Scheduling;

public static class DueDateReminderSchedule
{
    public const string JobId = "task-due-date-reminder";
    public const string CronExpression = "0 1 * * *";

    public static TimeZoneInfo TimeZone { get; } =
        TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");

    public static void Register(IRecurringJobManager recurringJobs)
    {
        ArgumentNullException.ThrowIfNull(recurringJobs);

        recurringJobs.AddOrUpdate(
            JobId,
            Job.FromExpression<DueDateReminderJob>(job =>
                job.ExecuteAsync(CancellationToken.None)),
            CronExpression,
            new RecurringJobOptions
            {
                TimeZone = TimeZone
            });
    }
}
