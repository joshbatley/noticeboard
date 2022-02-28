using Amazon.S3;
using MediatR;
using Noticeboard.Core.Clients;
using Noticeboard.Core.Configuration;
using Noticeboard.Core.Handlers;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.Configure<S3Options>(builder.Configuration.GetSection("s3"));
builder.Services.AddSingleton<IS3Client, S3Client>();
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