# Purpose
This simple project is intended to provide the opportunity for storing and reading specific messages logged via the ```Microsoft.Extensions.Logging```.
These messages should be useful only for the consumer of your project, therefore exceptions, stack-traces, or any other sensitive data
should not be logged via this Logging Provider. 

# How to use
* [ConsoleApp](AJProds.MessageLoggerProvider/readme.md)
* [WebApp](AJProds.MessageLoggerProvider.HttpContext/readme.md)
* [See the test apps in the Test folder for ideas!](Test)

# Packages
* MessageLoggerProvider -> works via a custom solution with AsyncLocal
* MessageLoggerProvider.Core -> holds the common implementations
* MessageLoggerProvider.HttpContext -> works via the ```IHttpContextAccessor```

# References, ideas from
* https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
* https://github.com/dotnet/runtime/tree/35f87ecf04416831c5675617b2bda4e2a031592f/src/libraries/Microsoft.Extensions.Logging.Console
* https://learn.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1

# TODO
- [ ] Add a middleware to convert the MessageEntry objects to ProblemDetails 
-> e.g.: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails?source=recommendations&view=aspnetcore-7.0
- [ ] Publish nuget packages