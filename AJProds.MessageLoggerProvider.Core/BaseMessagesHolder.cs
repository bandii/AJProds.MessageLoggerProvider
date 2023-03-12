using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AJProds.MessageLoggerProvider;

/// <summary>
/// Stores the messages, and also adds a view on them.
/// </summary>
public class BaseMessagesHolder
{
    /// <summary>
    /// The highest level of log stored.
    /// </summary>
    public LogLevel Highest { get; protected set; } = LogLevel.None;

    /// <summary>
    /// A view of all the messages stored.
    /// </summary>
    public IReadOnlyCollection<MessageEntry> Messages => _messages;
    
    private readonly ConcurrentQueue<MessageEntry> _messages = new();

    /// <summary>
    /// Appends the given <paramref name="message"/> to the collection of logger messages.
    /// </summary>
    public virtual void AppendMessage([DisallowNull] MessageEntry message)
    {
        if (message.Level > Highest
         || Highest == LogLevel.None)
        {
            Highest = message.Level;
        }

        _messages.Enqueue(message);
    }
}