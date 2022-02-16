using Basket.Domain.Interfaces;

namespace Basket.Infrastructure.Interfaces
{
    public interface ILogHelper<TService, TEntity> where TService: class
    where TEntity: class, IEntity
    {
        void AnonymousCreatedCartLog(TEntity shoppingCardItem);
        void CreatedCartLog(TEntity shoppingCardItem);
        void CreationFailedLog(TEntity shoppingCardItem, string exception);
        void CreatedButCouldNotSendToQueue(TEntity shoppingCardItem, string exception);
    }
}