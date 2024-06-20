using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication19.Models;

namespace WebApplication19.Services
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>();

        public Task<User> Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            return Task.FromResult(user);
        }

        public Task<User> Register(string username, string password)
        {
            var user = new User { Username = username, Password = password };
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User> GetByUsername(string username)
        {
            var user = _users.SingleOrDefault(x => x.Username == username);
            return Task.FromResult(user);
        }

        public Task SaveRefreshToken(string username, string refreshToken)
        {
            var user = _users.SingleOrDefault(x => x.Username == username);
            if (user != null)
            {
                user.RefreshToken = refreshToken;
            }
            return Task.CompletedTask;
        }
    }
}