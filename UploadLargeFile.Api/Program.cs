using Microsoft.EntityFrameworkCore;
using UploadLargeFile.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

services.AddPooledDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(
        connectionString: configuration.GetConnectionString("Default"),
        sqlServerOptionsAction: optionBuilder =>
            optionBuilder
            .ExecutionStrategy(dependencies =>
                new SqlServerRetryingExecutionStrategy(
                    dependencies: dependencies,
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(1000),
                    errorNumbersToAdd: [3])
            ).MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name)
        );
});
services.AddScoped<AppDbContextScopedFactory>();
services.AddScoped(sp => sp.GetRequiredService<AppDbContextScopedFactory>().CreateDbContext());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
