using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventSphere.Business.Services;
using EventSphere.Business.Helper;

namespace EventSphere.API.Controllers
{
    [Authorize]
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
        [Authorize (Policy = "Admin"  )]
        public async Task<IActionResult> GetUsers()
        {
            var userClaims = User.Claims;
            var users = await _userService.GetAllUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UpdateUserDTO>>(users);
            return Ok(userDtos);
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var count = await _userService.GetUserCountAsync();
            return Ok(count);
        }

        [HttpGet("getUser/{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UpdateUserDTO>(user);
            return Ok(userDto);
        }

        [HttpPut("updateUser")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDto)
        {

            var existingUser = await _userService.GetUserByIdAsync(updateUserDto.Id);
            if (existingUser == null)

            await _userService.UpdateUserAsync(updateUserDto);
            return NoContent();
        }
        

        [HttpPatch("updateUserPassword/{id}")]
        [Authorize(Policy = "All")]
        public async Task<IActionResult> UpdateUserPassword(int id, UpdatePasswordDto updatePasswordDto)
        {
            try

            {
                await _userService.UpdateUserPasswordAsync(id, updatePasswordDto);
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         
        }

        [HttpDelete("deleteUser/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}