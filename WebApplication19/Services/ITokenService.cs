using WebApplication19.Models;

namespace WebApplication19.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}