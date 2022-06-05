using API_Authentication__Minimal_API.Models;

namespace API_Authentication__Minimal_API.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new(){Username="Tesfay_Admin",EmailAddress="tesfay@gmail.com",Password="myPass1234",GivenName="tes",
            Surname="tadesse",Role="Administrator"},
            new(){Username="john_Standard",EmailAddress="jhon@gmail.com",Password="myPass1234",GivenName="tes",
            Surname="cina",Role="Standard"},
            new(){Username="lydia",EmailAddress="jhon@gmail.com",Password="myPass1234",GivenName="tes",
            Surname="cina",Role="student"},
        };
    }
}
