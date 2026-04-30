using IMDBSample.Models.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Repository.Interfaces
{
    public interface IActorRepository
    {
        int Add(Actor actor);
        bool Delete(int id);
        IEnumerable<Actor> GetAll();
        Actor GetById(int id);
        bool Update(int id, Actor actor);
        IEnumerable<Actor> GetByMovieId(int movieId);
    }
}