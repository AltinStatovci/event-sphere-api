using EventSphere.API.Filters;
using EventSphere.Business.Validator;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
            var categories = await _eventCategoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventCategoryDto>> GetCategoryById(int id)
        {
            var category = await _eventCategoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<EventCategoryDto>> AddCategory(EventCategoryDto categoryDTO)
        {
            var validationResult = _validator.Validate(categoryDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ValidationException(errors);
            }

            var addedCategory = await _eventCategoryService.AddCategoryAsync(categoryDTO);
            return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.Id }, addedCategory);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, EventCategoryDto categoryDTO)
        {
            if (id != categoryDTO.Id)
            {
                return BadRequest();
            }

            await _eventCategoryService.UpdateCategoryAsync(categoryDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _eventCategoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
