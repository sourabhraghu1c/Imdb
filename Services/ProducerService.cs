using IMDBSample.Exceptions;
using IMDBSample.Helpers;
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
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;

        public ProducerService(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;
        }

        public IEnumerable<ProducerResponse> GetAll()
        {
            var producers = _producerRepository.GetAll();

            if (producers == null || !producers.Any()) 
            {
                throw new EntityNotFoundException("No producers found");
            }

            return producers.Select(producer => new ProducerResponse
            {
                Id = producer.Id,
                Name = producer.Name,
                Bio = producer.Bio,
                DOB = producer.DOB.ToString("yyyy-MM-dd"),
                Gender = producer.Gender
            });
        }

        public ProducerResponse GetById(int id)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid producer id.");

            var producer = _producerRepository.GetById(id);

            if (producer == null)
                throw new EntityNotFoundException($"Producer with ID {id} not found");

            return new ProducerResponse
            {
                Id = producer.Id,
                Name = producer.Name,
                Bio = producer.Bio,
                DOB = producer.DOB.ToString("yyyy-MM-dd"),
                Gender = producer.Gender
            };
        }

        public int Add(ProducerRequest request)
        {
            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            PersonValidator.Validate(
                request.Name,
                request.Bio,
                request.DOB,
                request.Gender
            );

            var producer = new Producer
            {
                Name = request.Name.Trim(),
                Bio = request.Bio,
                DOB = request.DOB,
                Gender = request.Gender.ToUpper()
            };

            return _producerRepository.Add(producer);
        }

        public bool Update(int id, ProducerRequest request)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid producer id.");

            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            PersonValidator.Validate(
                request.Name,
                request.Bio,
                request.DOB,
                request.Gender
            );

            var existingProducer = _producerRepository.GetById(id);

            if (existingProducer == null)
                throw new EntityNotFoundException($"Producer with ID {id} not found.");

            var producer = new Producer
            {
                Name = request.Name.Trim(),
                Bio = request.Bio,
                DOB = request.DOB,
                Gender = request.Gender.ToUpper()
            };

            return _producerRepository.Update(id, producer);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new InvalidRequestDataException("Invalid producer id.");

            var existingProducer = _producerRepository.GetById(id);

            if (existingProducer == null)
                throw new EntityNotFoundException($"Producer with ID {id} not found.");

            return _producerRepository.Delete(id);
        }
    }
}