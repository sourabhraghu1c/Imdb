using IMDBSample.Models.Db;
using IMDBSample.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace IMDBSample.Repository
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(IOptions<ConnectionString> options) : base(options)
        {
        }

        public User GetByEmail(string email)
        {
            var sql = @"
                        SELECT 
                            Id,
                            Name,
                            Email,
                            Password,
                            Role
                        FROM Users
                        WHERE Email = @Email";

            return QuerySingle<User>(sql, new { Email = email });
        }

        public int Create(User user)
        {
            var sql = @"
                        INSERT INTO Users
                        (
                            Name,
                            Email,
                            Password,
                            Role
                        )
                        VALUES
                        (
                            @Name,
                            @Email,
                            @Password,
                            @Role
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT)";


            return ExecuteScalar<int>(sql, user);
        }
    }
}