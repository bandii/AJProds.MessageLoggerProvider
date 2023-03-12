using System.Collections.Concurrent;

namespace AJProds.MessageLoggerProvider;

[ProviderAlias("MessageLoggerProvider")]
internal sealed class MessageLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, MessageLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly IMessagesAccessor _messagesAccessor;

    public MessageLoggerProvider(IMessagesAccessor messagesAccessor)
    {
        _messagesAccessor = messagesAccessor;
    }

    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName,
                             _ => new MessageLogger(_messagesAccessor));

    public void Dispose()
    {
        _loggers.Clear();
    }
}