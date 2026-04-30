using IMDBSample.Exceptions;
using IMDBSample.Models.Common;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IMDBSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] SignupRequest request)
        {
            
                var userId = _authService.Signup(request);

                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = "User created successfully",
                    Data = userId
                });
            
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            
                var response = _authService.Login(request);

                return Ok(new ApiResponse<AuthResponse>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = response
                });
            
        }
    }
}