using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Tribuno.Kafka.Producer
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducer(IOptions<KafkaSettings> options)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<T>(string topic, T message)
        {
            var json = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = json
            });
        }
    }
}