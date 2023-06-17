using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services
    .AddHealthChecksUI(settings =>
settings.AddHealthCheckEndpoint("Service 1", "https://localhost:5001/health"))
   .AddSqlServerStorage(connectionString: builder.Configuration["ConnectionStrings:SqlConnection:HealthConnectDB"]!);
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
    connectionString: builder.Configuration["ConnectionStrings:SqlConnection:HealthConnectDB"]!,
    healthQuery: "SELECT 1",
    name: "MS SQL Server Check",
    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
    tags: new string[] { "db", "sql", "sqlserver" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options => options.UIPath = "/health-ui");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
