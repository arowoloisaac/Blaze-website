﻿namespace startup_trial.Services.TokenService
{
    public interface ITokenStorageService
    {
        void LogoutToken(Guid identifier);
        bool CheckIfTokenIsLoggedOut(Guid identifier);
    }
}
