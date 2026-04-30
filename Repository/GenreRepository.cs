using IMDBSample.Models.Db;
using IMDBSample.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDBSample.Repository
{
    public class GenreRepository : BaseRepository, IGenreRepository
    {
        public GenreRepository(IOptions<ConnectionString> options) : base(options)
        {
        }

        public IEnumerable<Genre> GetAll()
        {
            var sql = @"
                        SELECT 
                            Id,
                            Name
                        FROM Genres";

            return  Query<Genre>(sql);
        }

        public Genre GetById(int id)
        {
            var sql = @"
                        SELECT 
                            Id,
                            Name
                        FROM Genres
                        WHERE Id = @Id";

            return QuerySingle<Genre>(sql, new { Id = id });
        }

        public int Add(Genre genre)
        {
            var sql = @"
                        INSERT INTO Genres
                        (
                            Name
                        )
                        VALUES
                        (
                            @Name
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return ExecuteScalar<int>(sql, genre);
        }

        public bool Update(int id, Genre genre)
        {
            var sql = @"
                        UPDATE Genres
                        SET
                            Name = @Name
                        WHERE Id = @Id";

            var rows = Execute(sql, new
            {
                Id = id,
                genre.Name
            });

            return rows > 0;
        }

        public bool Delete(int id)
        {
            var sql = @"
                        DELETE FROM Genres
                        WHERE Id = @Id";

            var rows = Execute(sql, new { Id = id });

            return rows > 0;
        }

        public IEnumerable<Genre> GetByMovieId(int movieId)
        {
            var sql = @"
                    SELECT 
                        g.Id,
                        g.Name
                    FROM Genres g
                    INNER JOIN MovieGenreMapping mgm 
                        ON g.Id = mgm.GenreId
                    WHERE mgm.MovieId = @MovieId";

            return Query<Genre>(sql, new { MovieId = movieId });
        }
    }
}