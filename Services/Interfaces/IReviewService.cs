using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Services.Interfaces
{
    public interface IReviewService
    {
        IEnumerable<ReviewResponse> GetAll(int movieId);

        ReviewResponse GetById(int movieId, int id);

        int Add(int movieId, ReviewRequest request);

        bool Update(int movieId, int id, ReviewRequest request);

        bool Delete(int movieId, int id);
    }
}