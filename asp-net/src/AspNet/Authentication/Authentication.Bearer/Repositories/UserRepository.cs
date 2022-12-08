using Authentication.Bearer.Models;

namespace Authentication.Bearer.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User? GetUserByEmail(string email)
        {
            if (email == "test@mail.com")
            {
                return new User(id: Guid.NewGuid(), firstName: "Tuan", lastName: "Le", email: "test@gmail.com", hashedPassword: "hashedPassword");
            }
            return default;
        }
    }
}
