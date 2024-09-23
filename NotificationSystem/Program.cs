using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationSystem;
using NotificationSystem.Entities;
using NotificationSystem.Repositories;
using NotificationSystem.Senders;
using NotificationSystem.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
var configuration = builder.Configuration;

services.AddPooledDbContextFactory<AppDbContext>(options =>
{
    options
        .UseNpgsql(
            connectionString: configuration.GetConnectionString("Default"),
            npgsqlOptionsAction: optionBuilder =>
                optionBuilder
                .ExecutionStrategy(dependencies =>
                    new NpgsqlRetryingExecutionStrategy(
                        dependencies: dependencies,
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(1000),
                        errorCodesToAdd: null)
                ).MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name)
        );
});
services.AddScoped<AppDbContextScopedFactory>();
services.AddScoped(sp => sp.GetRequiredService<AppDbContextScopedFactory>().CreateDbContext());

services.AddHangfire(config =>
{
    config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UsePostgreSqlStorage(options =>
        {
            options.UseNpgsqlConnection(configuration.GetConnectionString("Default"));
        });
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Register notification senders
services.AddScoped<INotificationSender, EmailSender>();
services.AddScoped<INotificationSender, SMSSender>();
services.AddScoped<INotificationSender, TelegramSender>();

// Register the factory
services.AddScoped<INotificationSenderFactory, NotificationSenderFactory>();

// Register the repository for outbox management
services.AddScoped<IOutboxRepository, OutboxRepository>();
services.AddScoped<IUserRepository, UserRepository>();

// Register services
services.AddScoped<INotificationService, NotificationService>();
services.AddScoped<IOutboxService, OutboxService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");

app.MapPost("/user", async ([FromBody]User user, AppDbContext dbContext) =>
{
    await dbContext.Users.AddAsync(user);
    await dbContext.SaveChangesAsync();
    return Results.Ok(user);
})
.WithName("CreateNewUser")
.WithOpenApi();

app.MapGet("/user/{id:int}", async (int id, AppDbContext dbContext) =>
{
    User? user = await dbContext.Users.FindAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.WithName("GetUserById")
.WithOpenApi();

app.MapPost("/user/{id:int}/send-notify", async (int id, AppDbContext dbContext, INotificationService notificationService) =>
{
    User? user = await dbContext.Users.FindAsync(id);
    if (user is null)
    {
        return Results.NotFound();
    }
    await notificationService.SendNotification($"[{DateTime.Now}] Hello, World!", user);
    return Results.NoContent();
})
.WithName("SengNotify")
.WithOpenApi();

RecurringJob.AddOrUpdate<IOutboxService>("job-handle-failed-notify", x => x.RetryFailedNotifications(), "*/5 * * * *");

app.Run();
