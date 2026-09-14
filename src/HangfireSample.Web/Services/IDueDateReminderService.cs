namespace HangfireSample.Web.Services;

public interface IDueDateReminderService
{
    Task SendDueSoonRemindersAsync(CancellationToken cancellationToken);
}
