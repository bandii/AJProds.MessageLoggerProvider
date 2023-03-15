using AJProds.MessageLoggerProvider;

using JetBrains.dotMemoryUnit;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable CS8602
#pragma warning disable CS8604

[assembly: DotMemoryUnit(FailIfRunWithoutSupport = false)]

namespace AJProds.LoggerProvider.Test;

class LoggingTests : BaseServiceTest
{
    public override void SetUp()
    {
        SharedServiceCollection.AddTransient<ILoggerProvider, MessageLoggerProvider.MessageLoggerProvider>();
        SharedServiceCollection.AddTransient<IMessagesAccessor, MessagesAccessor>();
    }

    [Test]
    public void ProviderCreatesLogger_OK()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        var provider = SharedServiceProvider.GetRequiredService<ILoggerProvider>();

        var initialCheckPoint = dotMemory.Check();

        // When
        var logger = provider.CreateLogger("category");

        // Then
        Assert.NotNull(logger);
        Assert.That(logger.GetType(), Is.EqualTo(typeof(MessageLogger)));

        dotMemory.Check(memory =>
                        {
                            Assert.That(memory.GetDifference(initialCheckPoint)
                                              .GetNewObjects(property => property.Namespace.Like("AJProds"))
                                              .ObjectsCount,
                                        Is.EqualTo(1));
                        });
    }

    [Test]
    public void SimpleLogging_ViaLogger_OK()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        var provider = SharedServiceProvider.GetRequiredService<ILoggerProvider>();
        var logger = provider.CreateLogger("category");

        // When
        logger.Log(LogLevel.Information,
                   new EventId(1234),
                   nameof(SimpleLogging_ViaLogger_OK));

        // Then
        Assert.NotNull(logMessagesAccessor.Messages);
        Assert.That(logMessagesAccessor.Messages.Single().Title, Is.EqualTo(nameof(SimpleLogging_ViaLogger_OK)));
        Assert.That(logMessagesAccessor.Messages.Single().ToString(),
                    Is.EqualTo("Information - SimpleLogging_ViaLogger_OK [1234]" + Environment.NewLine + ""));
    }

    [Test]
    public void ExceptionLogging_ViaLogger_OK()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        var provider = SharedServiceProvider.GetRequiredService<ILoggerProvider>();
        var logger = provider.CreateLogger("category");

        // When
        logger.LogInformation(new EventId(1234),
                              new Exception("Test exception"),
                              "Test message");

        // Then
        Assert.NotNull(logMessagesAccessor.Messages);
        Assert.That(logMessagesAccessor.Messages.Single().Title, Is.EqualTo("Test message"));
        Assert.That(logMessagesAccessor.Messages.Single().ToString(), Is.EqualTo("Information - Test message [1234]" + Environment.NewLine + ""));
    }

    [Test]
    public void SimpleLogging_ViaLogger_Disabled()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        var provider = SharedServiceProvider.GetRequiredService<ILoggerProvider>();
        var logger = provider.CreateLogger("category");

        // When
        logger.Log(LogLevel.None,
                   new EventId(1234),
                   nameof(SimpleLogging_ViaLogger_OK));

        // Then
        Assert.NotNull(logMessagesAccessor.Messages);
        Assert.IsEmpty(logMessagesAccessor.Messages);
    }

    [Test]
    public void SimpleLogging_ViaLogger_Scope()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        var provider = SharedServiceProvider.GetRequiredService<ILoggerProvider>();
        var logger = provider.CreateLogger("category");

        // When
        using (logger.BeginScope(new Dictionary<string, string>()))
        {
            logger.Log(LogLevel.None,
                       new EventId(1234),
                       nameof(SimpleLogging_ViaLogger_OK));
        }

        // Then
        Assert.NotNull(logMessagesAccessor.Messages);
        Assert.IsEmpty(logMessagesAccessor.Messages);
    }

    [Test]
    public void SimpleLogging_ViaAccessor_OK()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        // When
        logMessagesAccessor.AppendMessage(new MessageEntry
                                          {
                                              Level = LogLevel.Information,
                                              Title = nameof(SimpleLogging_ViaAccessor_OK)
                                          });

        // Then
        Assert.NotNull(logMessagesAccessor.Messages);
        Assert.That(logMessagesAccessor.Messages.Single().Title, Is.EqualTo(nameof(SimpleLogging_ViaAccessor_OK)));
        Assert.That(logMessagesAccessor.Messages.Single().ToString(), Is.EqualTo("Information - SimpleLogging_ViaAccessor_OK"));
    }

    [Test]
    public async Task SimpleLogging_ViaAccessor_MultiTask_OK()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        // When
        var messageGroup1 = Task.Factory.StartNew(() =>
                                                  {
                                                      for (int i = 0; i < 10; i++)
                                                      {
                                                          logMessagesAccessor.AppendMessage(new MessageEntry
                                                                                            {
                                                                                                Level = LogLevel.Information,
                                                                                                Title = "Group_1 - " + i
                                                                                            });

                                                          Task.Delay(10);
                                                      }
                                                  });
        var messageGroup2 = Task.Factory.StartNew(() =>
                                                  {
                                                      for (int i = 0; i < 10; i++)
                                                      {
                                                          logMessagesAccessor.AppendMessage(new MessageEntry
                                                                                            {
                                                                                                Level = LogLevel.Information,
                                                                                                Title = "Group_2 - " + i
                                                                                            });

                                                          Task.Delay(10);
                                                      }
                                                  });

        await messageGroup1;
        await messageGroup2;

        // Then
        Assert.NotNull(logMessagesAccessor.Messages);
        Assert.That(logMessagesAccessor.Messages.Count, Is.EqualTo(20));
    }
}