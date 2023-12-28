using AJProds.MessageLoggerProvider;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Building up the console app
using IHost host = Host.CreateDefaultBuilder(args)
                       .ConfigureLogging(builder =>
                                             // Use only the MessageLogger
                                             builder.ClearProviders()
                                                    .AddMessageLoggerProvider())
                       .UseConsoleLifetime()
                       .Build();

// Initialize the MessageLogger
// .. behind the scenes, it uses AsyncLocal, and the Init is the method, what initiate a collection for them messages.
var logMessagesAccessor = host.Services.GetRequiredService<IMessagesAccessor>();
logMessagesAccessor.Init();

// Setting some sample logs, note the AppSettings.json's Logging configuration.
var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogTrace("This line should not get hit, as the default minimum Loglevel is Information");
logger.LogDebug("This line should neither get hit, as the minimum Loglevel for this project is Error");

logger.LogWarning( "This line should get hit");
logger.LogError(1234, "This line should also get hit, note the event id in the log message!");

// Checking the logs stored so far
#pragma warning disable CS8602
foreach (var logEntry in logMessagesAccessor.Messages)
#pragma warning restore CS8602
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine(logEntry.ToString());
    Console.ResetColor();
}

host.Run();