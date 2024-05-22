using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
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
        public async Task<ActionResult<EventCategoryDto>> AddCategory(EventCategoryDto categoryDTO)
        {
            var addedCategory = await _eventCategoryService.AddCategoryAsync(categoryDTO);
            return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.ID }, addedCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, EventCategoryDto categoryDTO)
        {
            if (id != categoryDTO.ID)
            {
                return BadRequest();
            }

            await _eventCategoryService.UpdateCategoryAsync(categoryDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _eventCategoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
