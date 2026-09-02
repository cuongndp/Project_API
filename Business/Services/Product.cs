using Business.Dapper;
using Business.IServices;
using Dapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Business.Services
{
    public class Product: BaseApplicationService, IProduct
    {
        public Models_Context _context;
        public IConfiguration _configuration;
        public IDistributedCache _cache;
        public Product(Models_Context context, IConfiguration configuration, IServiceProvider serviceProvider, IDistributedCache cache) : base(serviceProvider)
        {
            _context = context;
            _configuration = configuration;
            _cache = cache;
        }
        public async Task<List<ProductViewModel>> GetProduct_Category()
        {
            // đầu tiên tạo key cho caching
            var cacheKey = "PRODUCT_CATEGORY";

            // tiếp theo là kiểm tra xem là trong caching nó có dữ liệu ko nếu ko thì dô db lấy
           
            var cacheData = await _cache.GetStringAsync(cacheKey);  // kiểm tra trong cache

            if (!string.IsNullOrEmpty(cacheData)) // nếu có trong cache thì lấy gán dô list 
            {
                var products_Cache = JsonSerializer.Deserialize<List<ProductViewModel>>(cacheData);   // chuyển nó từ json sang list<> rồi trả về luôn
                return products_Cache;
            }    
            //ngược lại nếu nó ko có trong cache thì lấy từ db ra và lưu nó dô caching để dùng cho lần sau
            var paramater = new DynamicParameters();
            var products = await connection.QueryAsync<ProductViewModel>("SP_Product_GetCategory", paramater);

            var json= JsonSerializer.Serialize(products); // chuyển nó qua json để lưu dô cache

            await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow= TimeSpan.FromDays(30)
            });
              
            return products;

        }
        public async Task<ProductDetailViewModel> Product_GetDetail(int id)
        {
            var cacheKey = "ProductDetail_" + id;
            var cacheData=await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheData))
            {
                var cacheDetail=JsonSerializer.Deserialize<ProductDetailViewModel>(cacheData);
                return cacheDetail;
            } 

            var paramater =new DynamicParameters();
            paramater.Add("@ProductID", id);
            var product_Detail = await connection.QueryMultipleAsync("SP_Product_GetDetail", paramater);
            var product = await product_Detail.ReadFirstOrDefaultAsync<ProductDetailViewModel>();
            var img = (await product_Detail.ReadAsync<string>()).ToList();
            var attributes = (await product_Detail.ReadAsync<ProductAttributeViewModel>()).ToList();
            product.Images = img;
            product.Attributes = attributes;

            var json = JsonSerializer.Serialize(product);
            await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
            });
    
            return product;
        }
        public async Task<Return_request> AddCartItem(Request_CartItem request, int id)
        {
            var product = await _context.dh_Product.FirstOrDefaultAsync(x => x.ProductID == request.ProductID);

            if (product == null)
            {
                return new Return_request()
                {
                    Seccess = false,
                    Message = "Không tìm thấy sản phẩm"
                };
            }
            if (request.Quantity <= 0)
            {
                return new Return_request()
                {
                    Seccess = false,
                    Message = "Số lượng phải lớn hơn 0"
                };
            }
            var cart = await _context.dh_Cart.FirstOrDefaultAsync(x => x.UserID == id);
            if (cart == null) return new Return_request()
            {
                Seccess = false,
                Message = "Không tìm thấy giỏ hàng"
            };
            var cartItem= await _context.dh_CartItem.FirstOrDefaultAsync(x=>x.CartID == cart.CartID && x.ProductID==request.ProductID);
            if (cartItem != null)
            {

                cartItem.Quantity += request.Quantity;
                cartItem.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return new Return_request()
                {

                    Seccess = true,
                    Message = "Cập nhật giỏ hàng thành công"
                };
            }
            var newCartItem = new dh_CartItem
            {
                CartID = cart.CartID,
                ProductID = request.ProductID,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow
            };
             _context.dh_CartItem.Add(newCartItem);
            await _context.SaveChangesAsync();
            return new Return_request()
            {
                Seccess = true,
                Message = "Sản phẩm đã thêm vào giỏ hàng"
            };
        }
        public async Task<List<CartItemViewModel>> GetCartItem(int id)
        {
            var cartID= await _context.dh_Cart.FirstOrDefaultAsync(x=>x.UserID==id);
            var paramater = new DynamicParameters();
            paramater.Add("@CartID", cartID.CartID);
            var cartItemData = await connection.QueryAsync<CartItemViewModel>("SP_Cart_GetCartItem", paramater);

            return cartItemData;
        }
    }
}
