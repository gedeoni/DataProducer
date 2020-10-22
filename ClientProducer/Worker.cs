using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ClientApi.DTOs;
using ClientApi.Interfaces;
using MediatR;

namespace ClientProducer
{

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Random _random;
        private readonly IMediator _mediator;
        private readonly IClientRepository _fakeClientRepo;
        private readonly IEventPublisher _rabbitMqEventPublisher;

        public Worker(ILogger<Worker> logger, IMediator mediator, IClientRepository fakeClientRepo,IEventPublisher rabbitMqEventPublisher)
        {
            _logger = logger;
            _random = new Random();
            _mediator = mediator;
            _fakeClientRepo = fakeClientRepo;
            _rabbitMqEventPublisher = rabbitMqEventPublisher;
            _fakeClientRepo.InitializeTable();
        }

        public FakeClientPayload GenerateRabbitMqPayload(FakeClient fClient){
            return new FakeClientPayload{
                Id = fClient.AccountNumber,
                Ref = "http://localhost:8080/api/countries/"+fClient.Country+"/accounts/"+fClient.AccountNumber.ToString(),
                DateTime = DateTime.Now
            };
        }

        public async Task<FakeClient> GenerateDynamicClient(){
            string [] countries = {"RW","UG", "MW", "BI", "KE", "TZ"};
            string country = countries[_random.Next(0, countries.Length-1)];
            int accountNumber = _random.Next(10000000,90000000);
            await Task.Delay(10000);
            return new FakeClient {
                AccountNumber = accountNumber,
                IsActive = "true",
                Country = country.ToLower()
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = _fakeClientRepo.InsertClient(GenerateDynamicClient().Result);
                _rabbitMqEventPublisher.PublishEvent((object) GenerateRabbitMqPayload(response.Result));
                _logger.LogInformation($"Results: '{response.Result.AccountNumber}'");
            }

            return Task.CompletedTask;
        }
    }

}
