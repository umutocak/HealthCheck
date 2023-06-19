# HealthCheck
In this project, I briefly mentioned how the Health Check structure is used with .net.

We add the Health Check section to our project with the section below.
```c#
builder.Services.AddHealthChecks();
```
If we want to add sql control to Health Check, we should use the following code:
```c#
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
    connectionString: builder.Configuration["ConnectionStrings:SqlConnection:HealthConnectDB"]!,
    healthQuery: "SELECT 1",
    name: "MS SQL Server Check",
    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
    tags: new string[] { "db", "sql", "sqlserver" });
```
We used the following code to add Health Check UI to our project:
```c#
builder.Services.AddHealthChecksUI();
```
We pass the relevant endpoint information to give the health check control to the UI part. In addition, we use the .AddSqlServerStorage() method to save the information on the UI side to the sql side.
```c#
// AspNetCore.HealthChecks.UI - AspNetCore.HealthChecks.UI.Client
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
builder.Services
    .AddHealthChecksUI(settings =>
settings.AddHealthCheckEndpoint("Service 1", "https://localhost:5001/health"))
   .AddSqlServerStorage(connectionString: builder.Configuration["ConnectionStrings:SqlConnection:HealthConnectDB"]!);
```
You don't need to save UI data only on sql. You can also use alternative options. Other options:
```c#
builder.Services.AddHealthChecksUI().AddInMemoryStorage(); // In-Memory
builder.Services.AddHealthChecksUI().AddPostgreSqlStorage("connectionString"); //PostgreSQL
builder.Services.AddHealthChecksUI().AddMySqlStorage("connectionString"); // Mysql
builder.Services.AddHealthChecksUI().AddSqliteStorage($"Data Source=sqlite.db"); // Sqlite
```
Libraries you need to add according to your usage preference:
```c#
AspNetCore.HealthChecks.UI.InMemory.Storage
AspNetCore.HealthChecks.UI.SqlServer.Storage
AspNetCore.HealthChecks.UI.SQLite.Storage
AspNetCore.HealthChecks.UI.PostgreSQL.Storage
AspNetCore.HealthChecks.UI.MySql.Storage
```



