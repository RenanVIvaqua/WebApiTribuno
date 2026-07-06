using System;
using System.Collections.Generic;
using System.Text;

namespace Tribuno.Kafka.Producer
{
    public interface IKafkaProducer
    {
        Task PublishAsync<T>(string topic, T message);
    }
}
