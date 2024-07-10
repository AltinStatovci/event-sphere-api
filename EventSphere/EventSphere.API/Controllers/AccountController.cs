using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using MapsterMapper;

using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountService;
        

        public AccountController(IAccountServices accountService)
        {
            _accountService = accountService;
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO createUserDto)
        {
            try
            {
                var createdUser = await _accountService.AddUserAsync(createUserDto);
             
                Log.Information("User created successfully: {@User}", createdUser);
                return Ok(createdUser);
            }
            catch (ArgumentException ex)
            {
                Log.Error("User creation failed due to invalid input: {@Error}", ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating user: ");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginDTO loginDto)
        {
            try
            {
                var token = await _accountService.AuthenticateAsync(loginDto);
                if (token == null)
                {
                    Log.Error("User authentication failed: invalid credentials");
                    return BadRequest(new { Error = "Invalid credentials" });
                }
                Log.Information("User authenticated successfully: {@Token}", token);
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while authenticating user ");
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}