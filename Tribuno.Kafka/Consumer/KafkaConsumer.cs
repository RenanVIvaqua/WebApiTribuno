using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Tribuno.Domain;
using Tribuno.Kafka.Contracts;
//using Tribuno.Application.OperacaoService;

namespace Tribuno.Kafka.Consumer
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly KafkaSettings settings;

       // private readonly IOperacaoService operacaoService;

        public KafkaConsumer(IOptions<KafkaSettings> options)
        {
            settings = options.Value;
           //his.operacaoService = operacaoService;
        }

        public Task ConsumeAsync(string topic, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe(topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = consumer.Consume(cancellationToken);
                var evento = JsonSerializer.Deserialize<OperacaoCriadaEvent>(result.Message.Value);

                Console.WriteLine($"Operação: {evento?.NomeOperacao}");
            }

            consumer.Close();
            return Task.CompletedTask;
        }
    }
}