using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Services.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<GenreResponse> GetAll();

        GenreResponse GetById(int id);

        int Add(GenreRequest request);

        bool Update(int id, GenreRequest request);

        bool Delete(int id);
        List<GenreResponse> GetByMovieId(int movieId);
    }
}