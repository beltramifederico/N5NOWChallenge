using Newtonsoft.Json;

namespace N5NOW.UserPermissions.Infrastructure.Streaming.Kafka
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Configuration;
    using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;

    namespace InventoryProducer.Services
    {
        public class KafkaProducer<T> : IKafkaProducer<T>
        {
            private readonly IConfiguration _configuration;

            private readonly IProducer<string, string> _producer;

            public KafkaProducer(IConfiguration configuration)
            {
                _configuration = configuration;

                var producerconfig = new ProducerConfig
                {
                    BootstrapServers = _configuration["Kafka:BootstrapServers"]
                };

                _producer = new ProducerBuilder<string, string>(producerconfig).Build();
            }

            public async Task ProduceAsync<T>(string topic, T value)
            {
                var kafkamessage = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = JsonConvert.SerializeObject(value)
                };

                await _producer.ProduceAsync(topic, kafkamessage);
            }

            public void Dispose()
            {
                _producer.Dispose();
            }
        }
    }
}
