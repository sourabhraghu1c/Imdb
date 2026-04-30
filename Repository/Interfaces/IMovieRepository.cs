using IMDBSample.Models.Db;
using System.Collections.Generic;

namespace IMDBSample.Repository.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();

        IEnumerable<Movie> GetByYear(int year);

        Movie GetById(int id);

        int Add(Movie movie, List<int> actorIds, List<int> genreIds);

        bool Update(int id, Movie movie, List<int> actorIds, List<int> genreIds);
        bool UpdatePoster(int id, string coverImage);

        bool Delete(int id);

        List<int> GetActorIds(int movieId);

        List<int> GetGenreIds(int movieId);
    }
}