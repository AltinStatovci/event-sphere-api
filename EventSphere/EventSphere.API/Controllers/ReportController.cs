using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllReports()
        {
            var report = await _reportService.GetAllReportsAsync();
            return Ok(report);
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetReportCount()
        {
            var count = await _reportService.GetReportCountAsync();
            return Ok(count);
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetReportId(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            return Ok(report);
        }
        [HttpGet("GetReportByUserId/{userId}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetReportByUserId(int userId)
        {
            var reports = await _reportService.GetReportByUserIdAsync(userId);
            return Ok(reports);
        }
        [HttpPost]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> Create(ReportDTO reportDTO)
        {
            var report = await _reportService.CreateAsync(reportDTO);
            return CreatedAtAction(nameof(GetReportId), new { id = reportDTO.reportId }, reportDTO);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> Update(int id, ReportDTO reportDTO)
        {
            await _reportService.UpdateAsync(id, reportDTO);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> Delete(int id)
        {
            await _reportService.DeleteAsync(id);
            return NoContent();
        }
        
    }
}
