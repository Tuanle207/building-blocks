using Authentication.Bearer.Models;

namespace Authentication.Bearer.Repositories
{
    public interface IUserRepository
    {
        User? GetUserByEmail(string email);
    }
}
