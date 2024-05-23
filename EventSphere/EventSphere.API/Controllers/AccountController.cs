using EventSphere.Business.Services.Interfaces;
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
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            var createdUser = await _accountService.AddUserAsync(user);
            var userDto = _mapper.Map<UserDTO>(createdUser);
            return Ok(userDto);
        }
    }
}
