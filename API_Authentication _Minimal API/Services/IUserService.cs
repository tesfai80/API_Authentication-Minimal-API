using API_Authentication__Minimal_API.Models;

namespace API_Authentication__Minimal_API.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
