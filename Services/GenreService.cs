using IMDBSample.Exceptions;
using IMDBSample.Models.Db;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Repository;
using IMDBSample.Repository.Interfaces;
using IMDBSample.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IMDBSample.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public IEnumerable<GenreResponse> GetAll()
        {
            var genres = _genreRepository.GetAll(); //never reurn null 

            if (!genres.Any())
                throw new EntityNotFoundException("No genres found.");

            return genres.Select(g => new GenreResponse
            {
                Id = g.Id,
                Name = g.Name
            });
        }

        public GenreResponse GetById(int id)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid genre id.");

            var genre = _genreRepository.GetById(id);

            if (genre == null)
                throw new EntityNotFoundException($"Genre with ID {id} not found.");

            return new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public int Add(GenreRequest request)
        {
            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidRequestDataException("Genre name is required.");

            var genre = new Genre
            {
                Name = request.Name.Trim()
            };

            return _genreRepository.Add(genre);
        }

        public bool Update(int id, GenreRequest request)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid genre id.");

            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidRequestDataException("Genre name is required.");

            var existingGenre = _genreRepository.GetById(id);

            if (existingGenre == null)
                throw new EntityNotFoundException($"Genre with ID {id} not found.");

            var genre = new Genre
            {
                Name = request.Name.Trim()
            };

            return _genreRepository.Update(id, genre);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid genre id.");

            var existingGenre = _genreRepository.GetById(id);

            if (existingGenre == null)
                throw new EntityNotFoundException($"Genre with ID {id} not found.");

            return _genreRepository.Delete(id);
        }

        public List<GenreResponse> GetByMovieId(int movieId)
        {
            var genres = _genreRepository.GetByMovieId(movieId);

            return genres.Select(g => new GenreResponse
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
        }
    }
}