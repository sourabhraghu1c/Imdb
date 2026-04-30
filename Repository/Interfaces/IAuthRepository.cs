using IMDBSample.Models.Db;
namespace IMDBSample.Repository.Interfaces
{
    public interface IAuthRepository
    {
        User GetByEmail(string email);

        int Create(User user);
    }
}
