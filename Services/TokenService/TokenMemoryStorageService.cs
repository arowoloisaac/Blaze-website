using startup_trial.Data;
using startup_trial.Models;

namespace startup_trial.Services.TokenService
{
    public class TokenMemoryStorageService : ITokenStorageService
    {
        private readonly HashSet<Guid> _tokens;

        public TokenMemoryStorageService()
        {
            _tokens = new HashSet<Guid>();
        }

        public bool CheckIfTokenIsLoggedOut(Guid identifier)
        {
            return _tokens.Contains(identifier);
        }

        public void LogoutToken(Guid identifier)
        {
            _tokens.Add(identifier);
        }
    }


    public class TokenDbStorageService : ITokenStorageService
    {
        private readonly ApplicationDbContext _context;

        public TokenDbStorageService(ApplicationDbContext context)
        {
            _context = context;
        }


        public bool CheckIfTokenIsLoggedOut(Guid identifier)
        {
            return _context.LogoutTokens.Any(t => t.Identifier == identifier && !t.DeleteDate.HasValue);
        }

        public void LogoutToken(Guid identifier)
        {
            _context.LogoutTokens.Add(new LogoutToken
            {
                Identifier = identifier,
                CreateDateTime = DateTime.UtcNow
            });
            _context.SaveChanges();
        }
    }
}
