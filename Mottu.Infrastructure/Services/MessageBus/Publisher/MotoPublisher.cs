using System.Text;
using System.Text.Json;
using MassTransit;
using Mottu.Application.Contracts.Messaging;
using Mottu.Application.Events;

namespace Mottu.Infrastructure.Services.MessageBus.Publisher;

public class MotoPublisher :  IMotoPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MotoPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public async Task PublishWithRetryAsync(MotoCreatedIntegrationEvent motoIntegrationEvent)
    {
        await _publishEndpoint.Publish(motoIntegrationEvent);
    }
}