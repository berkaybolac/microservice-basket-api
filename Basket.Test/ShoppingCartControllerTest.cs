using System.Collections.Generic;
using System.Net;
using Basket.API.Controllers;
using Basket.Domain.Entities;
using Basket.Domain.Result;
using Basket.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Basket.Test
{
    public class ShoppingCartControllerTest
    {
        private readonly Mock<IShoppingCartService> _mockService;
        private readonly Mock<ILogHelper<ShoppingCartController, ShoppingCart>> _mockLogHelper;
        private readonly Mock<IHttpContextAccessor> _mockHttp;
        private readonly ShoppingCartController _shoppingCartController;
        public ShoppingCartControllerTest()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/getBasket";
            context.Request.Host = new HostString("localhost");
            var obj = new HttpContextAccessor();
            obj.HttpContext = context;
            
            _mockService = new Mock<IShoppingCartService>();
            _mockLogHelper = new Mock<ILogHelper<ShoppingCartController, ShoppingCart>>();
            _mockHttp = new Mock<IHttpContextAccessor>();
            _shoppingCartController =
                new ShoppingCartController(_mockService.Object, obj, _mockLogHelper.Object);
        }
        
        [Fact]
        public async void CreateShoppingCartLoggedInPerson_ShoppingCartItemInsertedIfInStock_ReturnOkResult()
        {
            var shoppingCartDummySuccess = new ShoppingCart()
            {
                UserId = 10,
                ShoppingCartItems = new List<ShoppingCardItem>()
                {
                    new ShoppingCardItem()
                    {
                        ProductId = 1,
                        ProductName = "Computer",
                        ProductCategory = "Electronic",
                        ProductStockQuantity = 15,
                        Quantity = 1
                    }
                }
            };
            var resultWithEntiy = new ResultWithEntity<ShoppingCart>();
            resultWithEntiy.Entity = shoppingCartDummySuccess;
            resultWithEntiy.Result = true;
            _mockService.Setup(x => x.CreateAsync(shoppingCartDummySuccess)).ReturnsAsync(resultWithEntiy);
            var result = await _shoppingCartController.InsertAsync(shoppingCartDummySuccess) as ObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            _mockService.Verify(x=>x.CreateAsync(shoppingCartDummySuccess), Times.Once);
        }
        
        [Fact]
        public async void CreateShoppingCartLoggedInPerson_ShoppingCartItemInsertedIfInStock_ReturnBadRequestResult()
        {
            var shoppingCartDummyFail = new ShoppingCart()
            {
                UserId = 10,
                ShoppingCartItems = new List<ShoppingCardItem>()
                {
                    new ShoppingCardItem()
                    {
                        ProductId = 1,
                        ProductName = "Computer",
                        ProductCategory = "Electronic",
                        ProductStockQuantity = 15,
                        Quantity = 16
                    }
                }
            };
            var resultWithEntiy = new ResultWithEntity<ShoppingCart>();
            resultWithEntiy.Entity = shoppingCartDummyFail;
            resultWithEntiy.Result = false;
            _mockService.Setup(x => x.CreateAsync(shoppingCartDummyFail)).ReturnsAsync(resultWithEntiy);
            var result = await _shoppingCartController.InsertAsync(shoppingCartDummyFail) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
            _mockService.Verify(x=>x.CreateAsync(shoppingCartDummyFail), Times.Once);
        }
        
        [Fact]
        public void CreateShoppingCartAnonymousPerson_ShoppingCartItemInsertedIfInStock_ReturnOkResult()
        {
            var shoppingCartDummySuccess = new ShoppingCart()
            {
                ShoppingCartItems = new List<ShoppingCardItem>()
                {
                    new ShoppingCardItem()
                    {
                        ProductId = 1,
                        ProductName = "Computer",
                        ProductCategory = "Electronic",
                        ProductStockQuantity = 15,
                        Quantity = 11
                    }
                }
            };
            var resultWithEntiy = new ResultWithEntity<ShoppingCart>();
            resultWithEntiy.Entity = shoppingCartDummySuccess;
            resultWithEntiy.Result = true;
            _mockService.Setup(x => x.ValidateAddBasketControl(shoppingCartDummySuccess)).Returns(resultWithEntiy);
            var result = _shoppingCartController.AnonymousInsert(shoppingCartDummySuccess) as ObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            _mockService.Verify(x=>x.ValidateAddBasketControl(shoppingCartDummySuccess), Times.AtMost(2));
        }
        
        [Fact]
        public void CreateShoppingCartAnonymousPerson_ShoppingCartItemInsertedIfInStock_ReturnBadRequestResult()
        {
            var shoppingCartDummySuccess = new ShoppingCart()
            {
                ShoppingCartItems = new List<ShoppingCardItem>()
                {
                    new ShoppingCardItem()
                    {
                        ProductId = 1,
                        ProductName = "Computer",
                        ProductCategory = "Electronic",
                        ProductStockQuantity = 15,
                        Quantity = 16
                    }
                }
            };
            var resultWithEntiy = new ResultWithEntity<ShoppingCart>();
            resultWithEntiy.Entity = shoppingCartDummySuccess;
            resultWithEntiy.Result = false;
            _mockService.Setup(x => x.ValidateAddBasketControl(shoppingCartDummySuccess)).Returns(resultWithEntiy);
            var result = _shoppingCartController.AnonymousInsert(shoppingCartDummySuccess) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
            _mockService.Verify(x=>x.ValidateAddBasketControl(shoppingCartDummySuccess), Times.AtMost(2));
        }
    }
}
