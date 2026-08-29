using Business.IServices;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
