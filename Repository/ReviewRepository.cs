using IMDBSample.Models.Db;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Repository
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        public ReviewRepository(IOptions<ConnectionString> options) : base(options)
        {
        }

        public IEnumerable<Review> GetAll(int movieId)
        {
            var sql = @"
                        SELECT 
                            Id,
                            Message,
                            MovieId
                        FROM Reviews
                        WHERE MovieId = @MovieId";

            return Query<Review>(sql, new { MovieId = movieId });
        }

        public Review GetById(int movieId, int id)
        {
            var sql = @"
                        SELECT 
                            Id,
                            Message,
                            MovieId
                        FROM Reviews
                        WHERE Id = @Id
                        AND MovieId = @MovieId";

            return QuerySingle<Review>(sql, new
            {
                Id = id,
                MovieId = movieId
            });
        }

        public int Add(Review review)
        {
            var sql = @"
                        INSERT INTO Reviews
                        (
                            Message,
                            MovieId
                        )
                        VALUES
                        (
                            @Message,
                            @MovieId
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return ExecuteScalar<int>(sql, review);
        }

        public bool Update(int movieId, int id, Review review)
        {
            var sql = @"
                        UPDATE Reviews
                        SET
                            Message = @Message
                        WHERE Id = @Id
                        AND MovieId = @MovieId";

            var rows = Execute(sql, new
            {
                Id = id,
                MovieId = movieId,
                review.Message
            });

            return rows > 0;
        }

        public bool Delete(int movieId, int id)
        {
            var sql = @"
                        DELETE FROM Reviews
                        WHERE Id = @Id
                        AND MovieId = @MovieId";

            var rows = Execute(sql, new
            {
                Id = id,
                MovieId = movieId
            });

            return rows > 0;
        }
    }
}