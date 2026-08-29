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
    }
}
