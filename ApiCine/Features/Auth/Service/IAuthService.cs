namespace ApiCine.Features.Auth.Service {
    public interface IAuthService {
        string Login(string email, string password);
    }
}
