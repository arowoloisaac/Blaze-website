namespace Arowolo_Delivery_Project.Services.TokenService
{
    public interface ITokenStorageService
    {
        void LogoutToken(Guid identifier);
        bool CheckIfTokenIsLoggedOut(Guid identifier);
    }
}
