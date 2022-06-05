using API_Authentication__Minimal_API.Models;
using API_Authentication__Minimal_API.Repositories;

namespace API_Authentication__Minimal_API.Services
{
    public class UserService : IUserService
    {
        public User Get(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(o=>o.Username.Equals
            (userLogin.Username,StringComparison.OrdinalIgnoreCase)&& o.Password
            .Equals(userLogin.Password));
            return user;
        }
    }
}
