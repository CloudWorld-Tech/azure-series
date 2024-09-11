using MediatR;

namespace CloudWorld.Containers.EventHubs.Producer.Features.SendMessage;

public record SendMessageCommand(SendMessageRequest MessageRequest) : IRequest;