using Arowolo_Delivery_Project.Dtos.OrderDtos;
using Arowolo_Delivery_Project.Models;
﻿using Arowolo_Delivery_Project.Dtos.BasketDto;

namespace Arowolo_Delivery_Project.Services.OrderService
{
    public interface IOrderService
    {
        Task<GetOrderDto> GetOrderById(Guid OrderId, string UserId);

        Task<List<GetOrderInfoDto>> GetOrder(string UserId);

        Task PostOrder(CreateOrderDto model, string UserId);

        Task ConfirmOrder(Guid OrderId, string UserId);
    }
}
