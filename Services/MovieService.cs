using IMDBSample.Exceptions;
using IMDBSample.Helpers;
using IMDBSample.Models.Db;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Repository.Interfaces;
using IMDBSample.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDBSample.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorService _actorService;
        private readonly IGenreService _genreService;
        private readonly IProducerService _producerService;
        private readonly ISupabaseService _supabaseService;
        private readonly IMovieValidator _movieValidator;

        public MovieService(
            IMovieRepository movieRepository,
            IActorService actorService,
            IProducerService producerService,
            IGenreService genreService,
            ISupabaseService supabaseService,
            IMovieValidator movieValidator)
        {
            _movieRepository = movieRepository;
            _actorService = actorService;
            _producerService = producerService;
            _genreService = genreService;
            _supabaseService = supabaseService;
            _movieValidator = movieValidator;
        }

        public IEnumerable<MovieResponse> GetAll()
        {
            var movies = _movieRepository.GetAll();

            if (!movies.Any())
                throw new EntityNotFoundException("No movies found.");

            return movies.Select(MapMovieResponse);
        }

        public IEnumerable<MovieResponse> GetByYear(int year)
        {
            var movies = _movieRepository.GetByYear(year);

            if (!movies.Any())
                throw new EntityNotFoundException("No movies found for given year.");

            return movies.Select(MapMovieResponse);
        }

        public MovieResponse GetById(int id)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            var movie = _movieRepository.GetById(id);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {id} not found.");

            return MapMovieResponse(movie);
        }

        public int Add(MovieRequest request)
        {
            _movieValidator.Validate(request);

            var movie = new Movie
            {
                Name = request.Name.Trim(),
                YearOfRelease = request.YearOfRelease,
                Plot = request.Plot.Trim(),
                CoverImage = request.CoverImage,
                ProducerId = request.ProducerId
            };

            return _movieRepository.Add(movie, request.ActorIds, request.GenreIds);
        }

        public bool Update(int id, MovieRequest request)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            var existingMovie = _movieRepository.GetById(id);

            if (existingMovie == null)
                throw new EntityNotFoundException($"Movie with ID {id} not found.");

            _movieValidator.Validate(request);

            var movie = new Movie
            {
                Name = request.Name.Trim(),
                YearOfRelease = request.YearOfRelease,
                Plot = request.Plot.Trim(),
                CoverImage = request.CoverImage,
                ProducerId = request.ProducerId
            };

            return _movieRepository.Update(id, movie, request.ActorIds, request.GenreIds);
        }

        public async Task<bool> UploadPosterAsync(int id, IFormFile file)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            var movie = _movieRepository.GetById(id);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {id} not found.");

            var imageUrl = await _supabaseService.UploadMoviePosterAsync(id, file);

            return _movieRepository.UpdatePoster(id, imageUrl);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            var movie = _movieRepository.GetById(id);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {id} not found.");

            return _movieRepository.Delete(id);
        }

        private MovieResponse MapMovieResponse(Movie movie)
        {
            var producer = _producerService.GetById(movie.ProducerId);

            var actors = _actorService.GetByMovieId(movie.Id);
            var genres = _genreService.GetByMovieId(movie.Id);

            return new MovieResponse
            {
                Id = movie.Id,
                Name = movie.Name,
                YearOfRelease = movie.YearOfRelease,
                Plot = movie.Plot,
                CoverImage = movie.CoverImage,

                Producer = producer,
                Actors = actors,
                Genres = genres
            };
        }
    }
}