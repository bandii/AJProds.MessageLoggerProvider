using System.Text;

namespace AJProds.MessageLoggerProvider;

/// <summary>
/// A single chunk of human-readable information, that could be processed by the consumers
/// </summary>
[Serializable]
public record MessageEntry
{
    private const string DefaultTitle = "Unkown";

    public LogLevel Level { get; init; } = LogLevel.None;

    /// <summary>
    /// A simple, short mandatory message
    /// </summary>
    public string Title { get; init; } = DefaultTitle;

    /// <summary>
    /// The id of the event
    /// </summary>
    public EventId? EventId { get; init; }

    /// <inheritdoc />
    public override string ToString()
    {
        var builder = new StringBuilder(Level.ToString())
                     .Append(" - ")
                     .Append(Title);

        if (EventId.HasValue)
        {
            builder.Append(" [")
                   .Append(EventId.Value.Id)
                   .AppendLine("]")
                   .Append(EventId.Value.Name);
        }

        return builder.ToString();
    }
}