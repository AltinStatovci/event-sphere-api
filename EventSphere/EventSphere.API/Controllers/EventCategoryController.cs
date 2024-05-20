using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class EventCategoryController : ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService;

        public EventCategoryController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventCategory>>> GetCategories()
        {
            var categories = await _eventCategoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventCategory>> GetCategory(int id)
        {
            var category = await _eventCategoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] EventCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest();
            }

            var createdCategory = await _eventCategoryService.CreateCategoryAsync(categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.ID }, createdCategory);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] EventCategoryDto categoryDto)
        {
            if (id == 0 || categoryDto == null)
            {
                return BadRequest();
            }

            await _eventCategoryService.UpdateCategoryAsync(id, categoryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _eventCategoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
