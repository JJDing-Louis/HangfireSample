using HangfireSample.Web.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace HangfireSample.Tests;

[TestFixture]
public sealed class ConsoleDueDateReminderServiceTests
{
    [Test]
    public async Task SendDueSoonRemindersAsync_CancelledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        var sut = new ConsoleDueDateReminderService(
            NullLogger<ConsoleDueDateReminderService>.Instance);
        using var cancellationSource = new CancellationTokenSource();
        await cancellationSource.CancelAsync();

        // Act
        Func<Task> act = () => sut.SendDueSoonRemindersAsync(
            cancellationSource.Token);

        // Assert
        await Assert.ThatAsync(
            act,
            Throws.InstanceOf<OperationCanceledException>());
    }
}
