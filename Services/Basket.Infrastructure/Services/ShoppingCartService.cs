using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Domain.Constants;
using Basket.Domain.Entities;
using Basket.Domain.Result;
using Basket.Infrastructure.Interfaces;

namespace Basket.Infrastructure.Services
{
    public class ShoppingCartService: IShoppingCartService
    {
        private readonly IMongoRepository<ShoppingCart> _shoppingCartCollection;
        public ShoppingCartService(IMongoRepository<ShoppingCart> shoppingCartCollection)
        {
            _shoppingCartCollection = shoppingCartCollection;
        }

        //Added for logged in person.
        public async Task<ResultWithEntity<ShoppingCart>> CreateAsync(ShoppingCart shoppingCart)
        {
            var stockControl = ValidateAddBasketControl(shoppingCart);
            if (stockControl.Entity.ShoppingCartItems.Any())
            {
                shoppingCart.ShoppingCartItems = stockControl.Entity.ShoppingCartItems;
                shoppingCart.CreatedTime = DateTime.Now;
                var insertedDatResult = await _shoppingCartCollection.Insert(shoppingCart);
                insertedDatResult.Message.AddRange(stockControl.Message);
            }
            return stockControl;
        }

        //Added for basket item and products validation.
        public ResultWithEntity<ShoppingCart> ValidateAddBasketControl(ShoppingCart shoppingCart)
        {
            var shoppingCartItems = new List<ShoppingCardItem>();
            var resultWithEntiy = new ResultWithEntity<ShoppingCart>();
            resultWithEntiy.Entity = shoppingCart;
            if (shoppingCart.ShoppingCartItems.Any())
            { 
                foreach (var shoppingCartItem in shoppingCart.ShoppingCartItems)
                {
                    if ((shoppingCartItem.Quantity > shoppingCartItem.ProductStockQuantity) && shoppingCartItem.Quantity > 0)
                    {
                        resultWithEntiy.Result = false;
                        resultWithEntiy.Message.Add($"{MessagesConstants.StockNotEnough}: {shoppingCartItem.ProductName} - {shoppingCartItem.ProductId}");
                    }
                    else if(shoppingCartItem.ProductId != 0 && shoppingCartItem.ProductName!=null)
                    {
                       shoppingCartItems.Add(shoppingCartItem);
                    }
                }
                shoppingCart.ShoppingCartItems = shoppingCartItems;
                return resultWithEntiy;
            }
            resultWithEntiy.Result = false;
            resultWithEntiy.Message.Add(MessagesConstants.BasketCannotBeEmpty);
            return resultWithEntiy;
        }
    }
}