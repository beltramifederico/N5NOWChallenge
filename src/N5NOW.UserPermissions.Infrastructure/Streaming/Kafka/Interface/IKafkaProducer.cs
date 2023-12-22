namespace N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface
{
    public interface IKafkaProducer<T>
    {
        Task ProduceAsync<T>(string topic, T value);
    }
}
