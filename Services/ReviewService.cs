using IMDBSample.Exceptions;
using IMDBSample.Models.Db;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Repository.Interfaces;
using IMDBSample.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IMDBSample.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;

        public ReviewService(IReviewRepository reviewRepository, IMovieRepository movieRepository)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
        }

        public IEnumerable<ReviewResponse> GetAll(int movieId)
        {
            if (movieId <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            var movie = _movieRepository.GetById(movieId);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {movieId} not found.");

            var reviews = _reviewRepository.GetAll(movieId);

            if (!reviews.Any())
                throw new EntityNotFoundException("No reviews found.");

            return reviews.Select(r => new ReviewResponse
            {
                Id = r.Id,
                Message = r.Message,
                MovieId = r.MovieId
            });
        }

        public ReviewResponse GetById(int movieId, int id)
        {
            if (movieId <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            if (id <= 0)
                throw new InvalidRequestDataException("Invalid review id.");

            var movie = _movieRepository.GetById(movieId);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {movieId} not found.");

            var review = _reviewRepository.GetById(movieId, id);

            if (review == null)
                throw new EntityNotFoundException($"Review with ID {id} not found.");

            return new ReviewResponse
            {
                Id = review.Id,
                Message = review.Message,
                MovieId = review.MovieId
            };
        }

        public int Add(int movieId, ReviewRequest request)
        {
            if (movieId <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            var movie = _movieRepository.GetById(movieId);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {movieId} not found.");

            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new InvalidRequestDataException("Review message is required.");

            var review = new Review
            {
                Message = request.Message.Trim(),
                MovieId = movieId
            };

            return _reviewRepository.Add(review);
        }

        public bool Update(int movieId, int id, ReviewRequest request)
        {
            if (movieId <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            if (id <= 0)
                throw new InvalidRequestDataException("Invalid review id.");

            var movie = _movieRepository.GetById(movieId);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {movieId} not found.");

            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new InvalidRequestDataException("Review message is required.");

            var existingReview = _reviewRepository.GetById(movieId, id);

            if (existingReview == null)
                throw new EntityNotFoundException($"Review with ID {id} not found.");

            var review = new Review
            {
                Message = request.Message.Trim(),
                MovieId = movieId
            };

            return _reviewRepository.Update(movieId, id, review);
        }

        public bool Delete(int movieId, int id)
        {
            if (movieId <= 0)
                throw new InvalidRequestDataException("Invalid movie id.");

            if (id <= 0)
                throw new InvalidRequestDataException("Invalid review id.");

            var movie = _movieRepository.GetById(movieId);

            if (movie == null)
                throw new EntityNotFoundException($"Movie with ID {movieId} not found.");

            var existingReview = _reviewRepository.GetById(movieId, id);

            if (existingReview == null)
                throw new EntityNotFoundException($"Review with ID {id} not found.");

            return _reviewRepository.Delete(movieId, id);
        }
    }
}