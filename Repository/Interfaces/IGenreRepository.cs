using IMDBSample.Models.Db;
using System.Collections.Generic;

namespace IMDBSample.Repository.Interfaces
{
    public interface IGenreRepository
    {
        int Add(Genre genre);
        bool Delete(int id);
        IEnumerable<Genre> GetAll();
        Genre GetById(int id);
        bool Update(int id, Genre genre);
        IEnumerable<Genre> GetByMovieId(int movieId);
    }
}