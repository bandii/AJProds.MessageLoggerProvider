using System.Text;

using AJProds.MessageLoggerProvider;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AJProds.LoggerProvider.Test;

class ExtendedMessageEntryTests : BaseServiceTest
{
    public override void SetUp()
    {
        SharedServiceCollection.AddTransient<ILoggerProvider, MessageLoggerProvider.MessageLoggerProvider>();
        SharedServiceCollection.AddTransient<IMessagesAccessor, MessagesAccessor>();
    }

    [Test]
    public void ExtendedMessageLogging_ViaAccessor_OK()
    {
        // Given
        var logMessagesAccessor = SharedServiceProvider.GetRequiredService<IMessagesAccessor>();
        logMessagesAccessor.Init();

        // When
        logMessagesAccessor.AppendMessage(new MyCustomEntry
                                          {
                                              Level = LogLevel.Information,
                                              Title = nameof(ExtendedMessageLogging_ViaAccessor_OK),
                                              Target = "My.UI.Page.Button"
                                          });

        // Then
        Assert.NotNull(logMessagesAccessor.Messages);
        Assert.That(logMessagesAccessor.Messages.Single().Title, Is.EqualTo(nameof(ExtendedMessageLogging_ViaAccessor_OK)));
        Assert.That(logMessagesAccessor.Messages.Single().ToString(), Is.EqualTo("Information - ExtendedMessageLogging_ViaAccessor_OK\r\nMy.UI.Page.Button"));
    }
}

record MyCustomEntry: MessageEntry
{
    /// <summary>
    /// A sample for extending the MessageEntry type.
    /// </summary>
    public string Target { get; init; } = string.Empty;

    /// <inheritdoc />
    public override string ToString() =>
        new StringBuilder(Level.ToString())
           .Append(" - ")
           .AppendLine(Title)
           .Append(Target)
           .ToString();
}