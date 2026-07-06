using Tribuno.Application.Constants;
using Tribuno.Kafka.Consumer;

namespace Tribuno.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IKafkaConsumer _consumer;

        public Worker(IKafkaConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ConsumeAsync(KafkaTopics.OperacaoCriada, stoppingToken);
        }
    }
}
