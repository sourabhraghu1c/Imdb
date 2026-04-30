using IMDBSample.Exceptions;
using IMDBSample.Helpers;
using IMDBSample.Models.Db;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Repository;
using IMDBSample.Repository.Interfaces;
using IMDBSample.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDBSample.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;

        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public IEnumerable<ActorResponse> GetAll()
        {
            var actors = _actorRepository.GetAll();

            if (actors == null || !actors.Any()) //repo never returns null 
            {
                throw new EntityNotFoundException("No actors found");
            }

            return actors.Select(actor => new ActorResponse
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DOB = actor.DOB.ToString("yyyy-MM-dd"),
                Gender = actor.Gender
            });
        }

        public ActorResponse GetById(int id)
        {

            if (id <= 0)
                throw new InvalidRequestDataException("Invalid actor id.");

            var actor = _actorRepository.GetById(id);

            if (actor == null)
                throw new EntityNotFoundException($"Actor with ID {id} not found");

            return new ActorResponse
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DOB = actor.DOB.ToString("yyyy-MM-dd"),
                Gender = actor.Gender
            };
        }

        public int Add(ActorRequest request)
        {
            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            PersonValidator.Validate(
                request.Name,
                request.Bio,
                request.DOB,
                request.Gender
            );

            var actor = new Actor
            {
                Name = request.Name.Trim(),
                Bio = request.Bio,
                DOB = request.DOB,
                Gender = request.Gender.ToUpper()
            };

            return _actorRepository.Add(actor);
        }

        public bool Update(int id, ActorRequest request)
        {
            
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid actor id.");

            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            PersonValidator.Validate(
                request.Name,
                request.Bio,
                request.DOB,
                request.Gender
            );

            
            var existingActor = _actorRepository.GetById(id);

            if (existingActor == null)
                throw new EntityNotFoundException($"Actor with ID {id} not found.");

            
            var actor = new Actor
            {
                Name = request.Name.Trim(),
                Bio = request.Bio,
                DOB = request.DOB,
                Gender = request.Gender.ToUpper()
            };

            return _actorRepository.Update(id, actor);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid actor id.");

            var existingActor = _actorRepository.GetById(id);

            if (existingActor == null)
                throw new EntityNotFoundException($"Actor with ID {id} not found.");

            return _actorRepository.Delete(id);
        }

        public List<ActorResponse> GetByMovieId(int movieId)
        {
            var actors = _actorRepository.GetByMovieId(movieId);

            return actors.Select(a => new ActorResponse
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                DOB = a.DOB.ToString("yyyy-MM-dd"),
                Gender = a.Gender
            }).ToList();
        }
    }
}