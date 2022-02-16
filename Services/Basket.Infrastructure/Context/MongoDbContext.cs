using Basket.Domain.Settings;
using MongoDB.Driver;

namespace Basket.Infrastructure.Context
{
    //Added for Generic Mongo Repository
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        
        public MongoDbContext(IMongoDbSettings mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.DatabaseName);
        }

        //Returns collection by TEntity type.
        public IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name.Trim());
        }
    }
}