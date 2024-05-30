using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Infrastructure.Repositories.TicketRepository;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ITicketRepository _ticketRepository;
        public TicketController(ITicketService ticketService, ITicketRepository ticketRepository)
        {
            _ticketService = ticketService;
            _ticketRepository = ticketRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var ticket = await _ticketService.GetAllTicketsAsync();
            return Ok(ticket);
        }
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetTicketId(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            return Ok(ticket);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TicketDTO ticketDTO)
        {
            var ticket = await _ticketService.CreateAsync(ticketDTO);
            return CreatedAtAction(nameof(GetTicketId), new {id = ticketDTO.ID}, ticketDTO);
        }
        [HttpPut ("{id}")]
        public async Task<IActionResult> Update(int id, TicketDTO ticketDTO)
        {
            await _ticketService.UpdateAsync(id, ticketDTO);
            return NoContent();
        }
        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("{id}/event")]
        public async Task<IActionResult> GetTicketByEventId(int id)
        {
            var ticket = await _ticketRepository.GetTicketByEvent(id);
            return Ok(ticket);
        }
    }
}
