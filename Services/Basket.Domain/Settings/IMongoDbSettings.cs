namespace Basket.Domain.Settings
{
    public interface IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}