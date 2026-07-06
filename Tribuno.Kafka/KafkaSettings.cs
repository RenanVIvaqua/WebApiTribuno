using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tribuno.Kafka
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = string.Empty;

        public string GroupId { get; set; } = string.Empty;

        public bool EnableAutoCommit { get; set; } = true;

        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;
    }
}
