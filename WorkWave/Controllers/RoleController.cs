using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;
using System.Buffers.Text;
using System;
using WorkWave.Controllers;
using WorkWave.DBModels;
using WorkWave.Dtos.RoleDto;
using WorkWave.Constants;
using WorkWave.Filters;
using System.Xml.Linq;
using WorkWave.Services;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: api/roles
        [HttpGet]
        [RoleFilter("admin")]
        public IActionResult GetRoles()
        {
           
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        // POST: api/roles
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            var role = new Role { Name = roleDto.Name };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Role created successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to create role.", Errors = result.Errors });
            }
        }

        // PUT: api/roles/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only users with the "Admin" role can access this action
        public async Task<IActionResult> UpdateRole(int id, RoleDto roleDto)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }

            role.Name = roleDto.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Role updated successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to update role.", Errors = result.Errors });
            }
        }

        // DELETE: api/roles/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only users with the "Admin" role can access this action
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Role deleted successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to delete role.", Errors = result.Errors });
            }
        }
    }
}
