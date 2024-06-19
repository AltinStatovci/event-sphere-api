using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var ticket = await _ticketService.GetAllTicketsAsync();
            return Ok(ticket);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTicketCount()
        {
            var count = await _ticketService.GetTicketCountAsync();
            return Ok(count);
        }
    

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketId(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            return Ok(ticket);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TicketDTO ticketDTO)
        {
            var ticket = await _ticketService.CreateAsync(ticketDTO);
            return CreatedAtAction(nameof(GetTicketId), new { id = ticketDTO.Id }, ticketDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TicketDTO ticketDTO)
        {
            await _ticketService.UpdateAsync(id, ticketDTO);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("{id}/event")]
        public async Task<IActionResult> GetTicketByEvent(int id)
        {
            var ticket = await _ticketService.GetTicketByEventId(id);
            return Ok(ticket);
        }
    }
}