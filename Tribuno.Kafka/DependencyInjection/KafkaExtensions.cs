using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tribuno.Kafka.Consumer;
using Tribuno.Kafka.Producer;

namespace Tribuno.Kafka.DependencyInjection
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafka(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

            return services;
        }
    }
}