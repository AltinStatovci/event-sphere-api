using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCategoriesController : ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService;

        public EventCategoriesController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
        }


        // GET: api/EventCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventCategory>>> GetEventCategories()
        {
            var eventCategories = await _eventCategoryService.GetAllEventCategoriesAsync();
            return Ok(eventCategories);
        }

        // GET: api/EventCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventCategory>> GetEventCategory(int id)
        {
            var eventCategory = await _eventCategoryService.GetEventCategoryByIdAsync(id);

            if (eventCategory == null)
            {
                return NotFound();
            }

            return Ok(eventCategory);
        }

        // POST: api/EventCategories
        [HttpPost]
        public async Task<ActionResult<EventCategory>> PostEventCategory(EventCategory eventCategory)
        {
            var newEventCategory = await _eventCategoryService.CreateEventCategoryAsync(eventCategory);
            return CreatedAtAction(nameof(GetEventCategory), new { id = newEventCategory.ID }, newEventCategory);
        }



        // PUT: api/EventCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventCategory(int id, EventCategory eventCategory)
        {
            if (id != eventCategory.ID)
            {
                return BadRequest();
            }

            await _eventCategoryService.UpdateEventCategoryAsync(eventCategory);

            return NoContent();
        }

        // DELETE: api/EventCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventCategory(int id)
        {
            await _eventCategoryService.DeleteEventCategoryAsync(id);
            return NoContent();
        }
    }
}
