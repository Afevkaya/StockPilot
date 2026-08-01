using StockPilot.Application.DependencyInjection;
using StockPilot.Persistence.DependencyInjection;
using StockPilot.Persistence.Migrations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddPersistence(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    DatabaseMigrator databaseMigrator = app.Services.GetRequiredService<DatabaseMigrator>();
    databaseMigrator.Migrate();
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "StockPilot API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
