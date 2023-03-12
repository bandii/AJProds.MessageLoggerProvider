namespace AJProds.MessageLoggerProvider;

/// <summary>
/// Gathers and holds those messages, what might be needed to be processed within the application.
/// </summary>
internal sealed class MessageLogger : ILogger
{
    private readonly IMessagesAccessor _messagesAccessor;

    public MessageLogger(IMessagesAccessor messagesAccessor)
    {
        _messagesAccessor = messagesAccessor;
    }

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel,
                            EventId eventId,
                            TState state,
                            Exception? exception,
                            Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var title = formatter(state, null); // We do not process the exception on purpose!
        if (string.IsNullOrEmpty(title))
        {
            title = !string.IsNullOrEmpty(exception?.Message)
                        ? exception.Message
                        : eventId.ToString();
        }

        _messagesAccessor.AppendMessage(new MessageEntry
                                        {
                                            Level = logLevel,
                                            Title = title,
                                            EventId = eventId
                                        });
    }

    /// <summary>
    /// An empty scope without any logic
    /// </summary>
    internal sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();

        private NullScope()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}