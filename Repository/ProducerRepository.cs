using IMDBSample.Models.Db;
using IMDBSample.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace IMDBSample.Repository
{
    public class ProducerRepository : BaseRepository, IProducerRepository
    {
        public ProducerRepository(IOptions<ConnectionString> options) : base(options)
        {
        }

        public IEnumerable<Producer> GetAll()
        {
            var sql = @"
                        SELECT 
                            Id,
                            Name,
                            Bio,
                            Dob AS DOB,
                            Gender
                        FROM Producers";

            return Query<Producer>(sql);
        }

        public Producer GetById(int id)
        {
            var sql = @"
                        SELECT 
                            Id,
                            Name,
                            Bio,
                            Dob AS DOB,
                            Gender
                        FROM Producers
                        WHERE Id = @Id";

            return QuerySingle<Producer>(sql, new { Id = id });
        }

        public int Add(Producer producer)
        {
            var sql = @"
                        INSERT INTO Producers
                        (
                            Name,
                            Bio,
                            Dob,
                            Gender
                        )
                        VALUES
                        (
                            @Name,
                            @Bio,
                            @DOB,
                            @Gender
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return ExecuteScalar<int>(sql, producer);
        }

        public bool Update(int id, Producer producer)
        {
            var sql = @"
                        UPDATE Producers
                        SET
                            Name = @Name,
                            Bio = @Bio,
                            Dob = @DOB,
                            Gender = @Gender
                        WHERE Id = @Id";

            var rows = Execute(sql, new
            {
                Id = id,
                producer.Name,
                producer.Bio,
                producer.DOB,
                producer.Gender
            });

            return rows > 0;
        }

        public bool Delete(int id)
        {
            var sql = @"
                        DELETE FROM Producers
                        WHERE Id = @Id";

            var rows = Execute(sql, new { Id = id });

            return rows > 0;
        }
    }
}