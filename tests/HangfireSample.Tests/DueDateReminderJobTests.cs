using HangfireSample.Web.Jobs;
using HangfireSample.Web.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace HangfireSample.Tests;

[TestFixture]
public sealed class DueDateReminderJobTests
{
    [Test]
    public void Constructor_NullReminderService_ThrowsArgumentNullException()
    {
        Assert.That(
            () => new DueDateReminderJob(
                null!,
                NullLogger<DueDateReminderJob>.Instance),
            Throws.TypeOf<ArgumentNullException>()
                .With.Property("ParamName").EqualTo("reminderService"));
    }

    [Test]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        Assert.That(
            () => new DueDateReminderJob(new RecordingReminderService(), null!),
            Throws.TypeOf<ArgumentNullException>()
                .With.Property("ParamName").EqualTo("logger"));
    }

    [Test]
    public async Task ExecuteAsync_WhenCalled_InvokesReminderServiceOnce()
    {
        // Arrange
        var reminderService = new RecordingReminderService();
        var sut = new DueDateReminderJob(
            reminderService,
            NullLogger<DueDateReminderJob>.Instance);
        using var cancellationSource = new CancellationTokenSource();

        // Act
        await sut.ExecuteAsync(cancellationSource.Token);

        // Assert
        Assert.That(reminderService.CallCount, Is.EqualTo(1));
        Assert.That(
            reminderService.ReceivedCancellationToken,
            Is.EqualTo(cancellationSource.Token));
    }

    [Test]
    public async Task ExecuteAsync_WhenReminderServiceFails_PropagatesException()
    {
        // Arrange
        var sut = new DueDateReminderJob(
            new FailingReminderService(),
            NullLogger<DueDateReminderJob>.Instance);

        // Act
        Func<Task> act = () => sut.ExecuteAsync(CancellationToken.None);

        // Assert
        await Assert.ThatAsync(
            act,
            Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("模擬寄送失敗"));
    }

    private sealed class RecordingReminderService : IDueDateReminderService
    {
        public int CallCount { get; private set; }

        public CancellationToken ReceivedCancellationToken { get; private set; }

        public Task SendDueSoonRemindersAsync(
            CancellationToken cancellationToken)
        {
            CallCount++;
            ReceivedCancellationToken = cancellationToken;
            return Task.CompletedTask;
        }
    }

    private sealed class FailingReminderService : IDueDateReminderService
    {
        public Task SendDueSoonRemindersAsync(
            CancellationToken cancellationToken) =>
            Task.FromException(
                new InvalidOperationException("模擬寄送失敗"));
    }
}
