using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO createUserDto)
        {
            var createdUser = await _accountService.AddUserAsync(createUserDto);
            return Ok(createdUser);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginDTO loginDto)
        {
            var token = await _accountService.AuthenticateAsync(loginDto);
            return Ok(token);
        }
    }
}
