using System;
using System.Threading.Tasks;
using Basket.Domain.Interfaces;
using Basket.Domain.Result;
using Basket.Domain.Settings;
using Basket.Infrastructure.Context;
using Basket.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Basket.Infrastructure.Repository
{
    //Added for Services
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity:  class, IEntity 
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<TEntity> _mongoCollection;
        private readonly ILogHelper<MongoRepository<TEntity>, TEntity> _logHelper;
        private readonly ISendEndpointProviderHelper<TEntity> _sendEndpointProviderHelper;
        public MongoRepository(IMongoDbSettings mongoDbSettings, ILogHelper<MongoRepository<TEntity>, TEntity> logHelper, ISendEndpointProviderHelper<TEntity> sendEndpointProviderHelper)
        {
            _logHelper = logHelper;
            _sendEndpointProviderHelper = sendEndpointProviderHelper;
            _context = new MongoDbContext(mongoDbSettings);
            _mongoCollection = _context.GetCollection<TEntity>();
        }
        
        //Added for Services, includes logging, result messages.
        public async Task<ResultWithEntity<TEntity>> Insert(TEntity entity)
        {
            try
            {
                await _mongoCollection.InsertOneAsync(entity);
                _logHelper.CreatedCartLog(entity);
                await _sendEndpointProviderHelper.SendToQueue(entity);
                return new ResultWithEntity<TEntity>();
            }
            catch(Exception ex)
            {
                _logHelper.CreationFailedLog(entity, ex.Message);
                var result = new ResultWithEntity<TEntity>();
                result.Result = false;
                result.Message.Add(ex.Message);
                return result;
            }
        }
    }
}