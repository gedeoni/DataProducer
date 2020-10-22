using MediatR;
using ClientApi.Events;
using ClientApi.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using ClientApi.DTOs;
using System;

namespace ClientApi.Commands
{
    public class UpsertClient : INotificationHandler<ClientCreated>
    {
        private readonly IClientRepository _clientRepo;
        private readonly ILogger<UpsertClient> _logger;

        private readonly IEventPublisher _rabbitMqEventPublisher;

        public UpsertClient(IClientRepository clientRepo, ILogger<UpsertClient> logger, IEventPublisher rabbitMqEventPublisher){
            _clientRepo = clientRepo;
            _rabbitMqEventPublisher = rabbitMqEventPublisher;
            _logger = logger;
        }

        public FakeClientPayload GenerateRabbitMqPayload(FakeClient fClient){
            return new FakeClientPayload{
                Id = fClient.AccountNumber,
                Ref = "/api/clients/fakeclient/"+fClient.AccountNumber.ToString(),
                DateTime = DateTime.Now
            };
        }

        async public Task Handle(ClientCreated notification, CancellationToken cancellationToken)
        {
            var client =  await _clientRepo.InsertClient(notification.fakeClient);
            _logger.LogInformation($"Client Generated: {client}");
            await _rabbitMqEventPublisher.PublishEvent((object) GenerateRabbitMqPayload(client));
        }
    }
}