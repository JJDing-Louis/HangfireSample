using HangfireSample.Web.Services;

namespace HangfireSample.Web.Jobs;

public sealed class DueDateReminderJob
{
    private readonly IDueDateReminderService _reminderService;
    private readonly ILogger<DueDateReminderJob> _logger;

    public DueDateReminderJob(
        IDueDateReminderService reminderService,
        ILogger<DueDateReminderJob> logger)
    {
        ArgumentNullException.ThrowIfNull(reminderService);
        ArgumentNullException.ThrowIfNull(logger);

        _reminderService = reminderService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "開始檢查即將到期的工作項目，時間：{Time}",
            DateTimeOffset.Now);

        await _reminderService.SendDueSoonRemindersAsync(cancellationToken);
    }
}
