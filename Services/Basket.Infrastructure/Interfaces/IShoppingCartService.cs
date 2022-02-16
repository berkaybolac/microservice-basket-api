using System.Threading.Tasks;
using Basket.Domain.Entities;
using Basket.Domain.Result;

namespace Basket.Infrastructure.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ResultWithEntity<ShoppingCart>> CreateAsync(ShoppingCart shoppingCart);
        ResultWithEntity<ShoppingCart> ValidateAddBasketControl(ShoppingCart shoppingCart);
    }
}