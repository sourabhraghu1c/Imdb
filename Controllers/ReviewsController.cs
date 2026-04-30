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
    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult Get([FromRoute] int movieId)
        {
            
                var reviews = _reviewService.GetAll(movieId);

                return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
                {
                    Success = true,
                    Message = "Reviews fetched successfully",
                    Data = reviews
                });
           
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int movieId, int id)
        {
            
                var review = _reviewService.GetById(movieId, id);

                return Ok(new ApiResponse<ReviewResponse>
                {
                    Success = true,
                    Message = "Review fetched successfully",
                    Data = review
                });
            
        }

        [HttpPost]
        public IActionResult Create([FromRoute] int movieId, [FromBody] ReviewRequest request)
        {
            
                var id = _reviewService.Add(movieId, request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { movieId, id },
                    new ApiResponse<int>
                    {
                        Success = true,
                        Message = "Review created successfully",
                        Data = id
                    });
            
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int movieId, int id, [FromBody] ReviewRequest request)
        {
            
                var updated = _reviewService.Update(movieId, id, request);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Review updated successfully",
                    Data = updated
                });
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int movieId, int id)
        {
            
                var deleted = _reviewService.Delete(movieId, id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Review deleted successfully",
                    Data = deleted
                });
            
        }
    }
}