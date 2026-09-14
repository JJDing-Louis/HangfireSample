using Hangfire;
using Hangfire.InMemory;
using Hangfire.Storage;
using HangfireSample.Web.Scheduling;

namespace HangfireSample.Tests;

[TestFixture]
[NonParallelizable]
public sealed class DueDateReminderScheduleTests
{
    [Test]
    public void Register_NullManager_ThrowsArgumentNullException()
    {
        Assert.That(
            () => DueDateReminderSchedule.Register(null!),
            Throws.TypeOf<ArgumentNullException>()
                .With.Property("ParamName").EqualTo("recurringJobs"));
    }

    [Test]
    public void Register_WhenCalledTwice_KeepsOneScheduleWithExpectedSettings()
    {
        // Arrange
        using var storage = new InMemoryStorage();
        var recurringJobs = new RecurringJobManager(storage);

        // Act
        DueDateReminderSchedule.Register(recurringJobs);
        DueDateReminderSchedule.Register(recurringJobs);

        // Assert
        using IStorageConnection connection = storage.GetConnection();
        IReadOnlyList<RecurringJobDto> scheduledJobs =
            connection.GetRecurringJobs();
        Assert.That(scheduledJobs, Has.Count.EqualTo(1));
        RecurringJobDto scheduledJob = scheduledJobs.Single();

        Assert.Multiple(() =>
        {
            Assert.That(scheduledJob.Id, Is.EqualTo(DueDateReminderSchedule.JobId));
            Assert.That(
                scheduledJob.Cron,
                Is.EqualTo(DueDateReminderSchedule.CronExpression));
            Assert.That(scheduledJob.TimeZoneId, Is.EqualTo("Asia/Taipei"));
        });
    }
}
