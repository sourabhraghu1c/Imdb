using IMDBSample.Exceptions;
using IMDBSample.Models.Request;
using IMDBSample.Services.Interfaces;
using System.Linq;

namespace IMDBSample.Helpers
{
    public class MovieValidator : IMovieValidator
    {
        private readonly IActorService _actorService;
        private readonly IProducerService _producerService;
        private readonly IGenreService _genreService;

        public MovieValidator(
            IActorService actorService,
            IProducerService producerService,
            IGenreService genreService)
        {
            _actorService = actorService;
            _producerService = producerService;
            _genreService = genreService;
        }

        public void Validate(MovieRequest request)
        {
            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidRequestDataException("Movie name is required.");

            if (string.IsNullOrWhiteSpace(request.Plot))
                throw new InvalidRequestDataException("Plot is required.");

            // Duplicate actors
            if (request.ActorIds.Distinct().Count() != request.ActorIds.Count)
                throw new InvalidRequestDataException("Duplicate actors not allowed.");

            // Duplicate genres
            if (request.GenreIds.Distinct().Count() != request.GenreIds.Count)
                throw new InvalidRequestDataException("Duplicate genres not allowed.");

            // Producer validation
            var producer = _producerService.GetById(request.ProducerId);
            if (producer == null)
                throw new EntityNotFoundException("Producer not found.");

            // Actors validation
            foreach (var actorId in request.ActorIds)
            {
                var actor = _actorService.GetById(actorId); // create this method if not exists
                if (actor == null)
                    throw new EntityNotFoundException($"Actor with ID {actorId} not found.");
            }

            // Genres validation
            foreach (var genreId in request.GenreIds)
            {
                var genre = _genreService.GetById(genreId);
                if (genre == null)
                    throw new EntityNotFoundException($"Genre with ID {genreId} not found.");
            }
        }
    }
}