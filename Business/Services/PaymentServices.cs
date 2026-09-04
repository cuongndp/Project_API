using Business.Dapper;
using Business.IServices;
using Dapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class PaymentServices : BaseApplicationService, IPaymentServices
    {
        public Models_Context _Context;
        public PaymentServices(Models_Context context, IServiceProvider serviceProvider) :base(serviceProvider)
        {
            _Context = context;
        }
        public async Task<CheckoutPreviewResponse> CheckoutCart(Request_CheckoutCart request_Checkout)
        {
            var paramater = new DynamicParameters();
            paramater.Add("@CartItem", string.Join(",",request_Checkout.CartItemIDs));
            var checkoutCart = await  connection.QueryMultipleAsync("SP_CheckoutCart",paramater);
            var data = (await checkoutCart.ReadAsync<CheckoutItemViewModel>()).ToList();
            var items = await checkoutCart.ReadFirstOrDefaultAsync<CheckoutPreviewResponse>();
            items.Items = data;
            return items;
        }

        public async Task<List<dh_ShippingAddress>> Get_ShippingAddress(int id)
        {
            var listShippingAddress= await _Context.dh_ShippingAddress.
                Where(x=>x.UserID == id).ToListAsync();
            return listShippingAddress;
        }

        public async Task<Return_request> Insert_ShippingAddress(Request_ShippingAddress request, int id)
        {
            var newShippingAddress = new dh_ShippingAddress()
            {
                UserID = id,
                Address = request.Address,
                RecipientName = request.RecipientName,
                RecipientPhone = request.RecipientPhone

            };
             await _Context.AddAsync(newShippingAddress);
            await _Context.SaveChangesAsync();
            return new Return_request
            {
                Seccess = true,
                Message = "Thêm địa chỉ thành công"
            };
        }
    }
}
