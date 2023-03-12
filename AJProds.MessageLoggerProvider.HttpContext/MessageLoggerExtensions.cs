using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AJProds.MessageLoggerProvider;

public static class Extensions
{
    /// <summary>
    /// Registers the <see cref="IMessagesAccessor"/> and its dependencies to store, then
    /// access those messages, what the consumer wishes to work on. 
    /// </summary>
    /// <remarks>
    /// Example for usages: when you might need to gather, then process specific logs, then use the
    /// <see cref="IMessagesAccessor"/> for either appending, or viewing the <see cref="MessageEntry"/> messages.
    /// </remarks>
    public static ILoggingBuilder AddMessageLoggerProvider(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IMessagesAccessor, MessagesAccessor>());
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, MessageLoggerProvider>());

        return builder;
    }
}