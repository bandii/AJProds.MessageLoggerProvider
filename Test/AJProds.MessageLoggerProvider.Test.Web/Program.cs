
using AJProds.MessageLoggerProvider;

var builder = WebApplication.CreateBuilder(args);

// Register the Project's logger provider
builder.Logging
       .ClearProviders()
       .AddMessageLoggerProvider();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the Project's background services per request
app.UseMessageLoggerProvider();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// After the request got handled, logging out the current content
app.Use(async (context, next) =>
        {
            await next();

            var accessor = context.RequestServices.GetRequiredService<IMessagesAccessor>();

            Console.WriteLine("Request is at " + context.Request.Path);

            // Checking the logs stored so far
#pragma warning disable CS8602
            foreach (var logEntry in accessor.Messages)
#pragma warning restore CS8602
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(logEntry.ToString());
                Console.ResetColor();
            }
        });

// This is the sample request
app.MapGet("WeatherForecast",
           (ILoggerFactory loggerFactory) =>
           {
               // Setting some sample logs, note the appSettings.json's Logging configuration.
               var logger = loggerFactory.CreateLogger("WeatherForecast");

               logger.LogInformation("This line should not get hit, as the default minimum Loglevel is None");
               logger.LogWarning("This line should neither get hit, as the minimum Loglevel for this project is Error");

               logger.LogError("This line should get hit");
               logger.LogError(1234, "This line should also get hit, note the event id in the log message!");
           });

app.Run();