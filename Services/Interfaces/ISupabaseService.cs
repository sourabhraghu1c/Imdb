using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IMDBSample.Services.Interfaces
{
    public interface ISupabaseService
    {
        Task<string> UploadMoviePosterAsync(int movieId, IFormFile file);
    }
}
