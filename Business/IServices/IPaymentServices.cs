using DataAccess.Models;

namespace Business.IServices
{
    public interface IPaymentServices
    {
        Task<CheckoutPreviewResponse> CheckoutCart(Request_CheckoutCart request_Checkout);
        Task<Return_request> Insert_ShippingAddress(Request_ShippingAddress request, int id);
        Task<List<dh_ShippingAddress>> Get_ShippingAddress( int id);
    }
}
