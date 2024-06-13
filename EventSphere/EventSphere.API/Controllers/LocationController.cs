using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LocationController : ControllerBase
    {
    private readonly ILocationServices _locationServices;
      
        public LocationController(ILocationServices locationServices)
        {
            _locationServices = locationServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            var locations = await _locationServices.GetAllLocations();
            return Ok(locations);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationById(int id)
        {
            var location = await _locationServices.GetLocationById(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation(Location location)
        {
            var createdLocation = await _locationServices.AddLocation(location);
            return CreatedAtAction(nameof(GetLocationById), new { id = createdLocation.Id }, createdLocation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            await _locationServices.UpdateLocation(location);
            return Ok(location);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            await _locationServices.DeleteLocation(id);
            return NoContent();
        }

        [HttpGet("city/{city}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByCity(string city)
        {
            var locations = await _locationServices.GetLocationsByCity(city);
            return Ok(locations);
        }
        [HttpGet("country/{country}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByCountry(string country)
        {
            var locations = await _locationServices.GetLocationsByCountry(country);
            return Ok(locations);
        }
    }
}
