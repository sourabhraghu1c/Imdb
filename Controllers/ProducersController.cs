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
public class ProducersController : ControllerBase
{
    private readonly IProducerService _producerService;

    public ProducersController(IProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        
            var producers = _producerService.GetAll();

            return Ok(new ApiResponse<IEnumerable<ProducerResponse>>
            {
                Success = true,
                Message = "Producers fetched successfully",
                Data = producers
            });
        
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        
            var producer = _producerService.GetById(id);

            return Ok(new ApiResponse<ProducerResponse>
            {
                Success = true,
                Message = "Producer fetched successfully",
                Data = producer
            });
        
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProducerRequest request)
    {
        
            var id = _producerService.Add(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new ApiResponse<int>
                {
                    Success = true,
                    Message = "Producer created successfully",
                    Data = id
                });
        
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProducerRequest request)
    {
        
            var updated = _producerService.Update(id, request);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Producer updated successfully",
                Data = updated
            });
        
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        
            var deleted = _producerService.Delete(id);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Producer deleted successfully",
                Data = deleted
            });
        
    }
}