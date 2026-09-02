using Business.IServices;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        public IProduct _product;
        public ProductController(IProduct product)
        {
            _product = product;
        }
        [HttpGet("Get_Product")]
        public async Task<IActionResult> Get_Product()
        {
            var product =await _product.GetProduct_Category();
            return Ok(product); 
        }
        [HttpGet("Get_Product_Detail")]
        public async Task<IActionResult> Get_Product_Detail(int id)
        {
            var product = await _product.Product_GetDetail(id);
            return Ok(product);
        }
        [Authorize]
        [HttpPost("Insert_CartItem")]
        public async Task<IActionResult> Insert_CartItem(Request_CartItem request)
        {
            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var req = await _product.AddCartItem(request, userID);
            return Ok(req);
        }
        [Authorize]
        [HttpGet("GetCartItem")]
        public async Task<IActionResult> GetCartItem()
        {
            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cartItem =await _product.GetCartItem(userID);
            return Ok(cartItem);
        }
    }
}
