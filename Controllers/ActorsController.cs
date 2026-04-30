using IMDBSample.Exceptions;
using IMDBSample.Models.Common;
using IMDBSample.Models.Request;
using IMDBSample.Models.Response;
using IMDBSample.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        
            var actors = _actorService.GetAll();

            return Ok(new ApiResponse<IEnumerable<ActorResponse>>
            {
                Success = true,
                Message = "Actors fetched successfully",
                Data = actors
            });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        
            var actor = _actorService.GetById(id);

            return Ok(new ApiResponse<ActorResponse>
            {
                Success = true,
                Message = "Actor fetched successfully",
                Data = actor
            });
        
    }

    [HttpPost]
    public IActionResult Create([FromBody] ActorRequest request)
    {
        
            var id = _actorService.Add(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new ApiResponse<int>
                {
                    Success = true,
                    Message = "Actor created successfully",
                    Data = id
                });
        
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ActorRequest request)
    {
        
            var updated = _actorService.Update(id, request);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Actor updated successfully",
                Data = updated
            });
        
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
       
            var deleted = _actorService.Delete(id);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Actor deleted successfully",
                Data = deleted
            });
        
    }
}