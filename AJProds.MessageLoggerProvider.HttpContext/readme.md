# Purpose
This simple project is intended to provide the opportunity for storing and reading specific messages logged via the ```Microsoft.Extensions.Logging```.
These messages should be useful only for the consumer of your project, therefore exceptions, stack-traces, or any other sensitive data
should not be logged via this Logging Provider. 

# How to use
1. Setup
   * Set up your Logger configuration. For examples see the Best practices below.
   * Register the Logging Provider as shown here:
   ```cs
     builder.Logging
     .ClearProviders()
     .AddMessageLoggerProvider();
   ```
   * Initialize the ```BaseMessageHolder``` per request via the ```app.UseMessageLoggerProvider();``` middleware.
2. Add messages via the ```ILogger``` injected, or via its extensions.
3. Process, transform (, or add more messages) via the ```IMessagesAccessor```

+1. See the [WebApp in the Test folder](../Test/AJProds.MessageLoggerProvider.Test.Web) for ideas!

# Best practices
* Always set the Logging Configuration, because you can easily get OutOfMemoryException!
   ```json
     {
      "Logging": {
        "MessageLoggerProvider": {
            "LogLevel": {
              "Default": "Information",
              "My.Awesome.BusinessCore.Project": "Warning"
            }
          }
        }
      }
   ```
* Verify the messages gathered, before you send them back to the consumer. These messages shall not contain sensitive content!
