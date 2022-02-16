using System;
using System.Linq;
using System.Threading.Tasks;
using Basket.Domain.Constants;
using Basket.Domain.Entities;
using Basket.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController: ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogHelper<ShoppingCartController, ShoppingCart> _logHelper;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IHttpContextAccessor httpContextAccessor, ILogHelper<ShoppingCartController, ShoppingCart> logHelper)
        {
            _shoppingCartService = shoppingCartService;
            _httpContextAccessor = httpContextAccessor;
            _logHelper = logHelper;
        }
        
        //Added for logged in person.
        [HttpPost("InsertAsync")]
        public async Task<IActionResult> InsertAsync(ShoppingCart shoppingCart)
        {
            var result = await _shoppingCartService.CreateAsync(shoppingCart);
            if (result.Result)
            {
                return Ok(result.ToJson());
            }
            return BadRequest(result.ToJson());
        }
        
        //Added for anonymous person.
        [HttpPost("AnonymousInsert")]
        public IActionResult AnonymousInsert(ShoppingCart shoppingCart)
        {
            if (_httpContextAccessor.HttpContext == null) return BadRequest();
            
            var result = _shoppingCartService.ValidateAddBasketControl(shoppingCart);
            shoppingCart.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
            
            if (!result.Result) return BadRequest(result.ToJson());
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieConstants.CartId, shoppingCart.Id,
                new CookieOptions {Expires = DateTime.Now.AddDays(30)});
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieConstants.CartItems, shoppingCart.ShoppingCartItems.Select(p=>p.ProductId).ToJson(),
                new CookieOptions {Expires = DateTime.Now.AddDays(30)});
            _logHelper.AnonymousCreatedCartLog(shoppingCart);
            return Ok(result.ToJson());
        }
    }
}