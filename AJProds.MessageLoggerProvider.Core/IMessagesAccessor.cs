using System.Diagnostics.CodeAnalysis;

namespace AJProds.MessageLoggerProvider;

/// <summary>
/// Entrypoint to access and append to the messages.
/// </summary>
public interface IMessagesAccessor
{
    /// <summary>
    /// A view of all the messages stored.
    /// </summary>
    public IReadOnlyCollection<MessageEntry>? Messages { get; }

    /// <summary>
    /// Appends the given <paramref name="message"/> to the collection of logger messages.
    /// </summary>
    public void AppendMessage([DisallowNull] MessageEntry message);

    /// <summary>
    /// Sets the given <paramref name="messagesHolder"/> as the messages collection holder.
    /// </summary>
    public void Init(BaseMessagesHolder? messagesHolder = null);
}