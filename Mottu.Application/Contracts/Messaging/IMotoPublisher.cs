using Mottu.Application.Events;

namespace Mottu.Application.Contracts.Messaging;

public interface IMotoPublisher
{
    Task PublishWithRetryAsync(MotoCreatedIntegrationEvent motoIntegrationEvent);
}