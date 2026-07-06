using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribuno.Kafka.Consumer
{
    public interface IKafkaConsumer
    {
        Task ConsumeAsync(
            string topic,
            CancellationToken cancellationToken);
    }
}
