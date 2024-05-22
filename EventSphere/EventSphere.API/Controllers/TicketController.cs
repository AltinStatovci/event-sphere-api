using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketServices _ticketServices;
        public TicketController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsAsync()
        {
            await _ticketServices.GetAllTicketsAsync();
            return Ok();
        }
        [HttpGet ("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketAsync(int id)
        {
            var ticket = await _ticketServices.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }
        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateAsync([FromBody] TicketDTO ticketDto)
        {
            if(ticketDto == null)
            {
                return BadRequest();
            }
            var ticket = await _ticketServices.AddTicketAsync(ticketDto);
            return CreatedAtAction(nameof(GetTicketAsync), new { id = ticket.ID }, ticket);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> EditAsync(int id, [FromBody] TicketDTO ticketDto)
        {
            if(id == 0 || ticketDto == null)
            {
                return BadRequest();
            }
            await _ticketServices.UdpateTicketAsync(id, ticketDto);
            return NoContent();
        }
        [HttpDelete ("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _ticketServices.DeleteTicketAsync(id);
            return NoContent();
        }
    }
}
