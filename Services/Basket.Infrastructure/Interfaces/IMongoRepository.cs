using System.Threading.Tasks;
using Basket.Domain.Interfaces;
using Basket.Domain.Result;

namespace Basket.Infrastructure.Interfaces
{
    public interface IMongoRepository<TEntity> where TEntity : class, IEntity
    {
        Task<ResultWithEntity<TEntity>> Insert(TEntity entity);
    }
}