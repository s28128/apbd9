using System.Threading.Tasks;
using WebApplication19.Models;
namespace WebApplication19.Services;

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task<User> Register(string username, string password);
    Task<User> GetByUsername(string username);
    Task SaveRefreshToken(string username, string refreshToken);
}