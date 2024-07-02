using EventSphere.API.Filters;
using EventSphere.Business.Validator;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ValidationException = EventSphere.API.Filters.ValidationException;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCategoryController : ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService;
        private readonly EventCategoryValidator _validator;
       

        public EventCategoryController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
            _validator = new EventCategoryValidator();
        
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventCategoryDto>>> GetAllCategories()
        {
            try
            {
                var categories = await _eventCategoryService.GetAllCategoriesAsync();
              
                return Ok(categories);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventCategoryDto>> GetCategoryById(int id)
        {
            try
            {
                var category = await _eventCategoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    Log.Error("Event category not found: {Id}", id);
                    return NotFound();
                }
              
                return Ok(category);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<EventCategoryDto>> AddCategory(EventCategoryDto categoryDTO)
        {
            try
            {
                var validationResult = _validator.Validate(categoryDTO);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    Log.Error("Validation failed for event category: {@Errors}", errors);
                    return BadRequest(new { Errors = errors });
                }

                var addedCategory = await _eventCategoryService.AddCategoryAsync(categoryDTO);
                Log.Information("Event category added successfully: {@Category}", addedCategory);
                return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.ID }, addedCategory);
            }
            catch (ValidationException ex)
            {
                Log.Error("Validation exception occurred while adding event category: {@Errors}", ex.Errors);
                return BadRequest(new { Errors = ex.Errors });
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while adding the event category: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, EventCategoryDto categoryDTO)
        {
            try
            {
                if (id != categoryDTO.ID)
                {
                    Log.Error("Mismatched ID in update request: {Id} != {CategoryId}", id, categoryDTO.ID);
                    return BadRequest(new { Error = "ID mismatch." });
                }

                await _eventCategoryService.UpdateCategoryAsync(categoryDTO);
                Log.Information("Event category updated successfully: {@Category}", categoryDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the event category: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _eventCategoryService.DeleteCategoryAsync(id);
                Log.Information("Event category deleted successfully: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the event category: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}
