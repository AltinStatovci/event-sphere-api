using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllTickets()
        {
            try
            {
                var tickets = await _ticketService.GetAllTicketsAsync();
               
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTicketCount()
        {
            try
            {
                var count = await _ticketService.GetTicketCountAsync();
               
                return Ok(count);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetTicketId(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                {
                    Log.Error("Ticket not found: {Id}", id);
                    return NotFound();
                }
              
                return Ok(ticket);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> Create(TicketDTO ticketDTO)
        {
            try
            {
                var createdTicket = await _ticketService.CreateAsync(ticketDTO);
                Log.Information("Ticket created successfully: {@Ticket}", createdTicket);
                return CreatedAtAction(nameof(GetTicketId), new { id = createdTicket.ID }, createdTicket);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the ticket: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> Update(int id, TicketDTO ticketDTO)
        {
            try
            {
                await _ticketService.UpdateAsync(id, ticketDTO);
                Log.Information("Ticket updated successfully: {@Ticket}", ticketDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the ticket: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ticketService.DeleteAsync(id);
                Log.Information("Ticket deleted successfully: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the ticket: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}/event")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetTicketByEvent(int id)
        {
            try
            {
                var tickets = await _ticketService.GetTicketByEventId(id);
               
                return Ok(tickets);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}
