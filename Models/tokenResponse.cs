
using System.Runtime.CompilerServices;

namespace startup_trial.Models
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
