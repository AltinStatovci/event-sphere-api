using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviews()
    {
        try
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            Log.Fatal("An error occurred while getting all reviews: {Message}", ex.Message);
            return StatusCode(500, new { Error = "An error occurred while processing your request." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(int id)
    {
        try
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                Log.Error("Review not found: {Id}", id);
                return NotFound();
            }

            return Ok(review);
        }
        catch (Exception ex)
        {
            Log.Fatal("An error occurred while getting the review by id: {Message}", ex.Message);
            return StatusCode(500, new { Error = "An error occurred while processing your request." });
        }
    }

    [HttpGet("organizer/{organizerId}")]
    public async Task<IActionResult> GetReviewsByOrganizerId(int organizerId)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByOrganizerIdAsync(organizerId);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            Log.Fatal("An error occurred while getting reviews by organizer id: {Message}", ex.Message);
            return StatusCode(500, new { Error = "An error occurred while processing your request." });
        }
    }

    [HttpGet("reviewer/{reviewerId}")]
    public async Task<IActionResult> GetReviewsByReviewerId(int reviewerId)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByReviewerIdAsync(reviewerId);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            Log.Fatal("An error occurred while getting reviews by reviewer id: {Message}", ex.Message);
            return StatusCode(500, new { Error = "An error occurred while processing your request." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(ReviewDTO reviewDTO)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        try
        {
            var review = await _reviewService.CreateAsync(reviewDTO);
            Log.Information("Review created successfully: {@Review} by {UserEmail}", reviewDTO, userEmail);
            return CreatedAtAction(nameof(GetReviewById), new { id = reviewDTO.Id }, reviewDTO);
        }
        catch (ArgumentException ex)
        {
            Log.Error("Argument exception occurred while creating review: {Message} by {UserEmail}", ex.Message, userEmail);
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            Log.Fatal("An error occurred while creating the review: {Message} by {UserEmail}", ex.Message, userEmail);
            return StatusCode(500, new { Error = "An error occurred while processing your request." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ReviewDTO reviewDTO)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        try
        {
            await _reviewService.UpdateAsync(id, reviewDTO);
            Log.Information("Review updated successfully: {@Review} by {UserEmail}", reviewDTO, userEmail);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            Log.Error("Invalid operation occurred while updating review: {Message} by {UserEmail}", ex.Message, userEmail);
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            Log.Fatal("An error occurred while updating the review: {Message} by {UserEmail}", ex.Message, userEmail);
            return StatusCode(500, new { Error = "An error occurred while processing your request." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        try
        {
            await _reviewService.DeleteAsync(id);
            Log.Information("Review deleted successfully: {Id} by {UserEmail}", id, userEmail);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Fatal("An error occurred while deleting the review: {Message} by {UserEmail}", ex.Message, userEmail);
            return StatusCode(500, new { Error = "An error occurred while processing your request." });
        }
    }
}
