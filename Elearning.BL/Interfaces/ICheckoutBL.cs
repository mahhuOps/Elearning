using Core.Interfaces;
using Core.Models;
using Elearning.Models;
namespace Elearning.BL.Interfaces
{
    public interface ICheckoutBL
    {
        Task<ServiceResponse> Checkout(Dictionary<string, object> payload);

        Task<ServiceResponse> ReceiveCheckoutResult(Dictionary<string, object> param);
    }
}
