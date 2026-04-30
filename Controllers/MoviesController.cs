using IMDBSample.Exceptions;
using IMDBSample.Models.Common;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBSample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int? year)
        {
            
                var movies = year.HasValue
                    ? _movieService.GetByYear(year.Value)
                    : _movieService.GetAll();

                return Ok(new ApiResponse<IEnumerable<MovieResponse>>
                {
                    Success = true,
                    Message = "Movies fetched successfully",
                    Data = movies
                });
            
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            
                var movie = _movieService.GetById(id);

                return Ok(new ApiResponse<MovieResponse>
                {
                    Success = true,
                    Message = "Movie fetched successfully",
                    Data = movie
                });
            
        }

        [HttpPost]
        public IActionResult Create([FromBody] MovieRequest request)
        {
            
                var id = _movieService.Add(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id },
                    new ApiResponse<int>
                    {
                        Success = true,
                        Message = "Movie created successfully",
                        Data = id
                    });
            
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MovieRequest request)
        {
            
                var updated = _movieService.Update(id, request);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Movie updated successfully",
                    Data = updated
                });
            
        }

        [HttpPatch("{id}/poster")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPoster(int id, IFormFile file)
        {
            
                var updated = await _movieService.UploadPosterAsync(id, file);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Poster updated successfully",
                    Data = updated
                });
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
                var deleted = _movieService.Delete(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Movie deleted successfully",
                    Data = deleted
                });
            
        }
    }
}