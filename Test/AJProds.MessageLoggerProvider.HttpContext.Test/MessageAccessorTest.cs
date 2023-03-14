using Microsoft.AspNetCore.Http;

using Moq;

#pragma warning disable CS8604
#pragma warning disable CS8618

namespace AJProds.MessageLoggerProvider.HttpContext.Test;

class MessageAccessorTest
{
    private Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    private DefaultHttpContext _httpContext;

    [SetUp]
    public void Setup()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _httpContext = new DefaultHttpContext();

        _mockHttpContextAccessor.Setup(accessor => accessor.HttpContext)
                                .Returns(_httpContext);
    }

    [Test]
    public void AccessorInitializes()
    {
        // Given
        var testee = new MessagesAccessor(_mockHttpContextAccessor.Object);

        // When
        testee.Init();

        // Then
        Assert.That(_httpContext.Items.Values.Single(), Is.TypeOf<BaseMessagesHolder>());
    }

    [Test]
    public void AccessorAppendsMessage_NonEmptyHolder()
    {
        // Given
        var testee = new MessagesAccessor(_mockHttpContextAccessor.Object);
        testee.Init();

        // When
        var message = new MessageEntry();
        testee.AppendMessage(message);

        // Then
        Assert.That(testee.Messages.Single(), Is.EqualTo(message));
    }

    [Test]
    public void AccessorAppendsMessage_InitializesHolder()
    {
        // Given
        var testee = new MessagesAccessor(_mockHttpContextAccessor.Object);
        // testee.Init();

        // When
        var message = new MessageEntry();
        testee.AppendMessage(message);

        // Then
        Assert.That(_httpContext.Items.Keys.Count, Is.EqualTo(1));
        Assert.That(_httpContext.Items.Values.Single(), Is.TypeOf<BaseMessagesHolder>());

        Assert.That(testee.Messages.Single(), Is.EqualTo(message));
    }
}