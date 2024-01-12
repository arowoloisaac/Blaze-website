
﻿using Arowolo_Delivery_Project.Enums;
﻿namespace Arowolo_Delivery_Project.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public DateTime DeliveryTime { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}
