using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EventSphere.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        

        public RoleController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
      
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateRole(Role role)
        {
            try
            {
                var createdRole = await _roleServices.AddRoleAsync(role);
                Log.Information("Role created successfully: {@Role}", createdRole);
                return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.ID }, createdRole);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while creating the role: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var role = await _roleServices.GetRoleByIdAsync(id);
                if (role == null)
                {
                    Log.Error("Role not found: {Id}", id);
                    return NotFound();
                }
              
                return Ok(role);
            }
            catch (Exception ex)
            {
          
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleServices.GetAllRolesAsync();
                
                return Ok(roles);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.ID)
            {
                Log.Error("Invalid ID or role data.");
                return BadRequest(new { Error = "Invalid ID or role data." });
            }

            try
            {
                await _roleServices.UpdateRoleAsync(role);
                Log.Information("Role updated successfully: {@Role}", role);
                return Ok(role);
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while updating the role: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _roleServices.DeleteRoleAsync(id);
                Log.Information("Role deleted successfully: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred while deleting the role: {@Error}", ex);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}
