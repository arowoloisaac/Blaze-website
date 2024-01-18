using startup_trial.Dtos.DeliveryDto;
using startup_trial.Dtos.OrderDtos;
using startup_trial.Enums;
using startup_trial.Models;

namespace startup_trial.Services.DeliveryService
{
    public interface IDeliveryService
    {
        //Task<ResponseString> AcquireOrder(Acquire arquire, string driverId);
        Task<string> AcquireOrder(Acquire arquire, string driverId);
        //Task<DeliveryProfileDto> DeliveryProfile(string UserId);
        Task<List<DeliveryProfileDto>> DeliveryProfile(string UserId);
    }
}
