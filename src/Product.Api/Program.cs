using Microsoft.EntityFrameworkCore;
using Product.Api.Data;
using Serilog;
using Prometheus;

Log.Logger = new LoggerConfiguration()
     .WriteTo.Console()
     .WriteTo.File(
        "/var/log/product-api/app-.log",
        rollingInterval: RollingInterval.Day
    ).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();

builder.Services
    .AddHealthChecks().AddNpgSql(builder.Configuration .GetConnectionString("DefaultConnection")!);

var app = builder.Build();

app.UseHttpMetrics();

app.UseSerilogRequestLogging();


app.UseSwagger();
app.UseSwaggerUI();

//uif (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();

app.Run();
