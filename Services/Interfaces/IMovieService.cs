using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Services.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieResponse> GetAll();

        MovieResponse GetById(int id);

        IEnumerable<MovieResponse> GetByYear(int year);

        int Add(MovieRequest request);

        bool Update(int id, MovieRequest request);
        Task<bool> UploadPosterAsync(int id, IFormFile file);

        bool Delete(int id);
    }
}