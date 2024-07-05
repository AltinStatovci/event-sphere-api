using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationServices _locationService;
       

        public LocationController(ILocationServices locationService)
        {
            _locationService = locationService;
          
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            try
            {
                var locations = await _locationService.GetAllLocations();
            
                return Ok(locations);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocationById(int id)
        {
            try
            {
                var location = await _locationService.GetLocationById(id);
                if (location == null)
                {
                    Log.Error("Location not found: {Id}", id);
                    return NotFound();
                }
               
                return Ok(location);
            }
            catch (Exception ex)
            {
            
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddLocation(Location location)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (location == null)
            {
                Log.Error("Invalid location data:");
                return BadRequest(new { Error = "Invalid location data." });
            }

            try
            {
                var createdLocation = await _locationService.AddLocation(location);
                Log.Information("Location added successfully: {@Location}  by {userEmail}", createdLocation , userEmail);
                return CreatedAtAction(nameof(GetLocationById), new { id = createdLocation.Id }, createdLocation);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while adding the location: by {userEmail} ", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (id != location.Id)
            {
                Log.Error("Invalid ID or location data:");
                return BadRequest(new { Error = "Invalid ID or location data." });
            }

            try
            {
                await _locationService.UpdateLocation(location);
                Log.Information("Location updated successfully: {@Location} by {userEmail}", location, userEmail);
                return Ok(location);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the location:  by {userEmail} ", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            try
            {
                await _locationService.DeleteLocation(id);
                Log.Information("Location deleted successfully: {Id}  by {userEmail}", id , userEmail);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the location: by {userEmail} ", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("city/{city}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByCity(string city)
        {
            try
            {
                var locations = await _locationService.GetLocationsByCity(city);
               
                return Ok(locations);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("country/{country}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByCountry(string country)
        {
            try
            {
                var locations = await _locationService.GetLocationsByCountry(country);
              
                return Ok(locations);
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}