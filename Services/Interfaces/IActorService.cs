using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Services.Interfaces
{
    public interface IActorService
    {
        IEnumerable<ActorResponse> GetAll();

        ActorResponse GetById(int id);

        int Add(ActorRequest request);

        bool Update(int id, ActorRequest request);

        bool Delete(int id);
        List<ActorResponse> GetByMovieId(int movieId);
    }
}