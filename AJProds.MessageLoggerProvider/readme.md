# Purpose
This simple project is intended to provide the opportunity for storing and reading specific messages logged via the ```Microsoft.Extensions.Logging```.
These messages should be useful only for the consumer of your project, therefore exceptions, stack-traces, or any other sensitive data
should not be logged via this Logging Provider. 

# How to use
1. Setup
   * Add the ```Microsoft.Extensions.Logging``` package to your project.
   * Set up your Logger configuration. For examples see the Best practices below.
   * Initialize this package in your application (either in the root or per request) via the ```IMessagesAccessor.Init()```.
   _Behind the scenes, this project uses AsyncLocal._
2. Add messages via the ```ILogger``` injected, or via its extensions.
3. Process, transform (, or add more messages) via the ```IMessagesAccessor```

+1. See the [ConsoleAPp in the Test folder](../Test/AJProds.MessageLoggerProvider.Test.Console) for ideas!

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
