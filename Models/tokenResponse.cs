
using System.Runtime.CompilerServices;

namespace Arowolo_Delivery_Project.Models
{

    public class tokenResponse
    {
        public string? Token { get; set; }

        public tokenResponse(string _token)
        {
            Token = _token;
        }
    }
}
