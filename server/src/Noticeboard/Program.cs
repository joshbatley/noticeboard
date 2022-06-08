using MediatR;
using Noticeboard.Core.Configuration;
using Noticeboard.Core.Handlers;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddS3Services(builder.Configuration);
builder.Services.AddDynamoDB(builder.Configuration);

builder.Services.AddMediatR(typeof(NoticeboardHandler));
builder.Services.AddSingleton<ILogger>(logger);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();