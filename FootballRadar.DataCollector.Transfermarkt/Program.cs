using FootballRadar.DataCollector.Kaggle;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.ConfigureServices(builder.Configuration, builder.Environment);

var host = builder.Build();
host.Run();