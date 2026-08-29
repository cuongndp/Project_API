using DataAccess.Models;

namespace Business.IServices
{
    public interface IProduct
    {
        Task<List<ProductViewModel>> GetProduct_Category();
        Task<ProductDetailViewModel> Product_GetDetail(int id);
    }
}
