using FootballRadar.DataCollector;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.ConfigureServices(builder.Configuration, builder.Environment);
var host = builder.Build();
host.Run();
