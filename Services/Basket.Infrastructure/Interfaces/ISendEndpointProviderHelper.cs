using System.Threading.Tasks;
using Basket.Domain.Interfaces;

namespace Basket.Infrastructure.Interfaces
{
    public interface ISendEndpointProviderHelper<TEntity> where TEntity:class, IEntity
    {
        Task SendToQueue(TEntity entity);
    }
}