# HangfireSample

這是一個給初學者操作的 ASP.NET Core 10 + Hangfire 範例。程式啟動時會註冊一個每天台北時間凌晨 1 點執行的週期工作，開發環境也會立即排入一個示範工作，讓你不用等到隔天就能看到執行結果。

## 執行環境

- .NET 10 SDK
- Hangfire.AspNetCore 1.8.25
- Hangfire.InMemory 1.0.0

## 啟動專案

```bash
dotnet restore HangfireSample.slnx
dotnet run --project src/HangfireSample.Web/HangfireSample.Web.csproj --urls http://localhost:5080
```

啟動後可以開啟：

- 首頁：<http://localhost:5080/>
- Hangfire Dashboard：<http://localhost:5080/hangfire>

終端機會看到示範工作開始與完成的 Log。Dashboard 的 `Recurring Jobs` 頁面則會顯示 `task-due-date-reminder`。

## 執行測試

```bash
dotnet test HangfireSample.slnx --configuration Release
```

測試會確認：

- 背景工作只呼叫一次提醒服務，並傳遞取消權杖。
- 提醒服務失敗時，工作會把例外往外拋，讓 Hangfire 能判定失敗並進行重試。
- 使用相同 ID 重複註冊排程時，只會保留一筆，且 Cron 與時區正確。

## 教學範例的限制

本專案使用記憶體儲存，關閉程式後工作資料會消失，適合省去資料庫設定、快速認識排程流程。正式環境應換成 SQL Server、PostgreSQL 或其他持久化儲存，並把 Dashboard 放在身分驗證與授權之後。提醒 Email 也必須處理去重，避免工作重試時重複寄送。
