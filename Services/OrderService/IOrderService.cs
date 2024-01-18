using startup_trial.Dtos.OrderDtos;
using startup_trial.Models;
﻿using startup_trial.Dtos.BasketDto;

namespace startup_trial.Services.OrderService
{
    public interface IOrderService
    {
        Task<GetOrderDto> GetOrderById(Guid OrderId, string UserId);

        Task<List<GetOrderInfoDto>> GetOrder(string UserId);

        Task<ResponseString> PostOrder(CreateOrderDto model, string UserId);

        Task<ResponseString> ConfirmOrder(Guid OrderId, string UserId);
    }
}
