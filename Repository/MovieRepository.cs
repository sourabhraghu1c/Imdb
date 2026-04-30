using IMDBSample.Models.Db;
using IMDBSample.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IMDBSample.Repository
{
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public MovieRepository(IOptions<ConnectionString> options) : base(options)
        {
        }

        public IEnumerable<Movie> GetAll()
        {
            var sql = @"
                        SELECT
                            Id,
                            Name,
                            YearOfRelease,
                            Plot,
                            CoverImage,
                            ProducerId
                        FROM Movies";

            return Query<Movie>(sql);
        }

        public Movie GetById(int id)
        {
            var sql = @"
                        SELECT
                            Id,
                            Name,
                            YearOfRelease,
                            Plot,
                            CoverImage,
                            ProducerId
                        FROM Movies
                        WHERE Id = @Id";

            return QuerySingle<Movie>(sql, new { Id = id });
        }

        public IEnumerable<Movie> GetByYear(int year)
        {
            var sql = @"
                        SELECT
                            Id,
                            Name,
                            YearOfRelease,
                            Plot,
                            CoverImage,
                            ProducerId
                        FROM Movies
                        WHERE YearOfRelease = @Year";

            return Query<Movie>(sql, new { Year = year });
        }

        public int Add(Movie movie, List<int> actorIds, List<int> genreIds)
        {
            var parameters = new
            {
                movie.Name,
                movie.YearOfRelease,
                movie.Plot,
                movie.ProducerId,
                movie.CoverImage,
                ActorIds = string.Join(",", actorIds),
                GenreIds = string.Join(",", genreIds)
            };

            return ExecuteScalarSp<int>(
                "usp_AddMovie",
                parameters
            );
        }

        public bool Update(int id, Movie movie, List<int> actorIds, List<int> genreIds)
        {
            var parameters = new
            {
                MovieId = id,
                movie.Name,
                movie.YearOfRelease,
                movie.Plot,
                movie.ProducerId,
                movie.CoverImage,
                ActorIds = string.Join(",", actorIds),
                GenreIds = string.Join(",", genreIds)
            };

            var rows = ExecuteSp(
                "usp_UpdateMovie",
                parameters
            );

            return rows > 0;
        }

        public bool UpdatePoster(int id, string coverImage)
        {
            var sql = @"UPDATE Movies 
                        SET CoverImage = @CoverImage
                        WHERE Id = @Id";

            var rows = Execute(sql, new
            {
                Id = id,
                CoverImage = coverImage
            });

            return rows > 0;
        }

        public bool Delete(int id)
        {
            var sql = "DELETE FROM Movies WHERE Id = @Id";

            var rows = Execute(sql, new { Id = id });

            return rows > 0;
        }

        public List<int> GetActorIds(int movieId)
        {
            var sql = @"
                        SELECT ActorId
                        FROM MovieActorMapping
                        WHERE MovieId = @MovieId";

            return Query<int>(sql, new { MovieId = movieId }).ToList();
        }

        public List<int> GetGenreIds(int movieId)
        {
            var sql = @"
                        SELECT GenreId
                        FROM MovieGenreMapping
                        WHERE MovieId = @MovieId";

            return Query<int>(sql, new { MovieId = movieId }).ToList();
        }
    }
}