using IMDBSample.Exceptions;
using IMDBSample.Helpers;
using IMDBSample.Models.Db;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Repository.Interfaces;
using IMDBSample.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;

namespace IMDBSample.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IAuthRepository authRepository,
            JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper; 
        }

        public int Signup(SignupRequest request)
        {
            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new InvalidRequestDataException("Email is required.");

            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new InvalidRequestDataException("Invalid email format.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidRequestDataException("Password is required.");

            var existingUser = _authRepository.GetByEmail(request.Email);

            if (existingUser != null)
                throw new InvalidRequestDataException("Email already registered.");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User"
            };

            return _authRepository.Create(user);
        }

        public AuthResponse Login(LoginRequest request)
        {

            if (request == null)
                throw new InvalidRequestDataException("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new InvalidRequestDataException("Email is required.");

            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new InvalidRequestDataException("Invalid email format.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidRequestDataException("Password is required.");

            var user = _authRepository
                .GetByEmail(request.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponse
            {
                Token = token
            };
        }
    }
}