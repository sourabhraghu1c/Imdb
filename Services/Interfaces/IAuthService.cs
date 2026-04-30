using IMDBSample.Models.Request;
using IMDBSample.Models.Response;

namespace IMDBSample.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponse Login(LoginRequest request);
        int Signup(SignupRequest request);
    }
}