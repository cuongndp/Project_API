using DataAccess.Models;

namespace Business.IServices
{
    public interface IProduct
    {
        Task<List<ProductViewModel>> GetProduct_Category();
        Task<ProductDetailViewModel> Product_GetDetail(int id);
        Task<Return_request> AddCartItem(Request_CartItem request, int id);
        Task<List<CartItemViewModel>> GetCartItem(int id);
    }
}
