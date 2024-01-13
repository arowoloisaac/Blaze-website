
﻿using startup_trial.Enums;
﻿namespace startup_trial.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public DateTime DeliveryTime { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}
