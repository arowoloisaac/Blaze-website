using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace startup_trial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        public RestaurantController()
        {
            
        }

        public async Task<IActionResult> RegisterRestaurant()
        {
            throw new NotImplementedException();    
        }

    }
}
