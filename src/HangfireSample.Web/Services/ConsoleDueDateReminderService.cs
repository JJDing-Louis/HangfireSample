namespace HangfireSample.Web.Services;

public sealed class ConsoleDueDateReminderService(
    ILogger<ConsoleDueDateReminderService> logger) : IDueDateReminderService
{
    public Task SendDueSoonRemindersAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation(
            "示範工作完成：目前沒有連接資料庫，也不會真的寄出 Email。");

        return Task.CompletedTask;
    }
}
