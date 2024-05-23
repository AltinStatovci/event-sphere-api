using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(userDtos);
        }

        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDto)
        {
            var existingUser = await _userService.GetUserByIdAsync(updateUserDto.ID);
            if (existingUser == null)
            {
                return NotFound();
            }

            var dateCreated = existingUser.DateCreated;
            _mapper.Map(updateUserDto, existingUser);
            existingUser.DateCreated = dateCreated;
            await _userService.UpdateUserAsync(existingUser);

            return NoContent();
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}