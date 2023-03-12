using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http;

namespace AJProds.MessageLoggerProvider;

/// <inheritdoc />
internal class MessagesAccessor: IMessagesAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;

    /// <inheritdoc />
    public IReadOnlyCollection<MessageEntry>? Messages => FindMessagesHolder()?.Messages;

    public MessagesAccessor(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public void AppendMessage([DisallowNull] MessageEntry message)
    {
        var messagesHolder = FindMessagesHolder();
        if (messagesHolder == null)
        {
            messagesHolder = new BaseMessagesHolder();
            _contextAccessor.HttpContext?.Items.Add(nameof(BaseMessagesHolder), messagesHolder);
        }
        
        messagesHolder.AppendMessage(message);
    }

    private BaseMessagesHolder? FindMessagesHolder()
    {
        return _contextAccessor.HttpContext?.Items[nameof(BaseMessagesHolder)] as BaseMessagesHolder;
    }

    /// <inheritdoc />
    public void Init(BaseMessagesHolder? messagesHolder = null)
    {
        if (messagesHolder == null)
        {
            var holder = new BaseMessagesHolder();
            _contextAccessor.HttpContext?.Items.Add(nameof(BaseMessagesHolder), holder);
        }
        else
        {
            _contextAccessor.HttpContext?.Items.Add(nameof(BaseMessagesHolder), messagesHolder);
        }
    }
}