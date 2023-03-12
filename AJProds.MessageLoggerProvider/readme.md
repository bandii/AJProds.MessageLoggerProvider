# Purpose
This simple project is intended to provide the opportunity for storing and reading specific messages logged via the ```Microsoft.Extensions.Logging```.
These messages should be useful only for the consumer of your project, therefore exceptions, stack-traces, or any other sensitive data
should not be logged via this Logging Provider. 

# How to use
1. Setup
   * Add the ```Microsoft.Extensions.Logging``` package to your project.
   * Set up your Logger configuration. For examples see the Best practices below.
   * Initialize this package in your application root via the ```IMessagesAccessor.Init()```.
   _Behind the scenes, this project uses AsyncLocal._
2. Add messages via the ```ILogger``` injected, or via its extensions.
3. Process, transform (, or add more messages) via the ```IMessagesAccessor```

+1. See the ConsoleApp in the Test folder for ideas!

# Best practices
* Always set the Logging Configuration, because you can easily get OutOfMemoryException!
   ```json
     {
      "Logging": {
        "MessageLoggerProvider": {
            "LogLevel": {
              "Default": "None",
              "My.Awesome.BusinessCore.Project": "Information"
            }
          }
        }
      }
   ```
* Verify the messages gathered, before you send them back to the consumer. These messages shall not contain sensitive content!

# References, ideas from:
* https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
* https://github.com/dotnet/runtime/tree/35f87ecf04416831c5675617b2bda4e2a031592f/src/libraries/Microsoft.Extensions.Logging.Console
