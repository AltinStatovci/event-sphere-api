using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogServices _logService;

        public LogController(ILogServices logService)
        {
            _logService = logService;
        }

        [HttpGet("{level}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<Logg>>> GetLogs(string level)
        {
            try
            {
                var logs = await _logService.GetLogsByLevelAsync(level);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request." + ex });
            }
        }
        
        [HttpDelete("deleteLog/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            try
            {
                await _logService.DeleteLogAsync(id);
                Log.Information("Log deleted successfully: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the Log: ");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
        
        [HttpDelete("deleteLog/all")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAllLogs()
        {
            try
            {
                await _logService.DeleteAllLogsAsync();
                Log.Information(" All Logs deleted successfully:" );
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the Log: ");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}