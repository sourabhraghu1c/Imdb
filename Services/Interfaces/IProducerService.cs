using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Services.Interfaces
{
    public interface IProducerService
    {
        IEnumerable<ProducerResponse> GetAll();

        ProducerResponse GetById(int id);

        int Add(ProducerRequest request);

        bool Update(int id, ProducerRequest request);

        bool Delete(int id);
    }
}