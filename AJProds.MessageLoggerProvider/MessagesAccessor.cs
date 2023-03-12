using System.Diagnostics.CodeAnalysis;

namespace AJProds.MessageLoggerProvider;

/// <inheritdoc />
internal class MessagesAccessor: IMessagesAccessor
{
    private static readonly AsyncLocal<BaseMessagesHolder> LoggerMessagesCurrent = new();

    /// <inheritdoc />
    public IReadOnlyCollection<MessageEntry>? Messages => LoggerMessagesCurrent.Value?.Messages;

    /// <inheritdoc />
    public void AppendMessage([DisallowNull] MessageEntry message)
    {
        LoggerMessagesCurrent.Value?.AppendMessage(message);
    }

    /// <inheritdoc />
    public void Init(BaseMessagesHolder? messagesHolder = null)
    {
        LoggerMessagesCurrent.Value = messagesHolder ?? new BaseMessagesHolder();
    }
}