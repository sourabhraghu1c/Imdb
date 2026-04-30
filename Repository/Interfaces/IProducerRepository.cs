using IMDBSample.Models.Db;
using System.Collections.Generic;

namespace IMDBSample.Repository.Interfaces
{
    public interface IProducerRepository
    {
        int Add(Producer producer);
        bool Delete(int id);
        IEnumerable<Producer> GetAll();
        Producer GetById(int id);
        bool Update(int id, Producer producer);
    }
}