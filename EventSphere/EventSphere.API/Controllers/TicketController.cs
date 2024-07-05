using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            try
            {
              
                var ticket = await _ticketService.CreateAsync(ticketDTO);
                Log.Information("Ticket created successfully: {@Ticket} by {userEmail}", ticketDTO , userEmail);
                return CreatedAtAction(nameof(GetTicketId), new { id = ticketDTO.ID }, ticketDTO);
              
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the ticket by {userEmail}", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> Update(int id, TicketDTO ticketDTO)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            try
            {
                await _ticketService.UpdateAsync(id, ticketDTO);
                Log.Information("Ticket updated successfully: {@Ticket} by {userEmail}", ticketDTO , userEmail);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the ticket  by {userEmail}", userEmail);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrOrganizer")]
        public async Task<IActionResult> Delete(int id)
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            try
            {
                await _ticketService.DeleteAsync(id);
                Log.Information("Ticket deleted successfully: {Id} by {userEmail}", id , userEmail);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the ticket by {userEmail}", userEmail);
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