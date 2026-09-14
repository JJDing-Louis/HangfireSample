using Hangfire;
using Hangfire.InMemory;
using HangfireSample.Web.Jobs;
using HangfireSample.Web.Scheduling;
using HangfireSample.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDueDateReminderService, ConsoleDueDateReminderService>();
builder.Services.AddScoped<DueDateReminderJob>();
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseHangfireDashboard("/hangfire");

app.MapGet("/", () =>
    "HangfireSample 正在執行。請開啟 /hangfire 查看背景工作。");

DueDateReminderSchedule.Register(
    app.Services.GetRequiredService<IRecurringJobManager>());

if (app.Environment.IsDevelopment() &&
    builder.Configuration.GetValue<bool>("Hangfire:RunDemoJobOnStartup"))
{
    app.Services
        .GetRequiredService<IBackgroundJobClient>()
        .Enqueue<DueDateReminderJob>(job =>
            job.ExecuteAsync(CancellationToken.None));
}

app.Run();
