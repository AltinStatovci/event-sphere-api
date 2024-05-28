using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase // Use ControllerBase instead of Controller for API controllers
    {
        private readonly ITicketServices _ticketServices;

        public TicketController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsAsync()
        {
            var tickets = await _ticketServices.GetAllTicketsAsync();
            return Ok(tickets); // Return the list of tickets
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult<Ticket>> CreateAsync(TicketDTO ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest();
            }

            var ticket = await _ticketServices.AddTicketAsync(ticketDto);
            // Ensure the route value key 'id' matches the parameter name in the GetTicketAsync method
            return CreatedAtAction(nameof(GetTicketAsync), new { id = ticket.ID }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditAsync(int id, [FromBody] TicketDTO ticketDto)
        {
            if (id == 0 || ticketDto == null)
            {
                return BadRequest();
            }

            var existingTicket = await _ticketServices.GetTicketByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            await _ticketServices.UdpateTicketAsync(id, ticketDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var existingTicket = await _ticketServices.GetTicketByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            await _ticketServices.DeleteTicketAsync(id);
            return NoContent();
        }
    }
}
