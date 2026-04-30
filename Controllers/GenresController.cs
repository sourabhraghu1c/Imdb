using IMDBSample.Exceptions;
using IMDBSample.Models.Common;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace IMDBSample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            
                var genres = _genreService.GetAll();

                return Ok(new ApiResponse<IEnumerable<GenreResponse>>
                {
                    Success = true,
                    Message = "Genres fetched successfully",
                    Data = genres
                });
            
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            
                var genre = _genreService.GetById(id);

                return Ok(new ApiResponse<GenreResponse>
                {
                    Success = true,
                    Message = "Genre fetched successfully",
                    Data = genre
                });
            
        }

        [HttpPost]
        public IActionResult Create([FromBody] GenreRequest request)
        {
            
                var id = _genreService.Add(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id },
                    new ApiResponse<int>
                    {
                        Success = true,
                        Message = "Genre created successfully",
                        Data = id
                    });
            
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] GenreRequest request)
        {
            
                var updated = _genreService.Update(id, request);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Genre updated successfully",
                    Data = updated
                });
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
                var deleted = _genreService.Delete(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Genre deleted successfully",
                    Data = deleted
                });
            
        }
    }
}