using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkWave.Constants;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos.JobOpeningDtos;
using WorkWave.Dtos.JobTypeDtos;
using WorkWave.Dtos.UserDtos;
using WorkWave.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthService _service;
        private readonly RoleService _roleService;
        private readonly IMapper _mapper; 
        private readonly IConfiguration _configuration;

        private readonly WorkwaveContext _context;

        public UserController(WorkwaveContext context, AuthService authService, RoleService roleService, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _service = authService;
            _roleService = roleService;
            _mapper = mapper;
            _configuration = configuration;
        }


        [HttpPost("registerEmployer")]
        public async Task<ActionResult> RegisterEmployer(EmployerRegistrationDto model)
        {
      
                User newUser = _mapper.Map<User>(model);
                Employer employer = _mapper.Map<Employer>(model);
                newUser.Role = RoleName.Employer;
                newUser.EmployerProfile = employer;
                var user = await _service.CreateUser(newUser, model.Password);
                await _roleService.AddRoleToUser(newUser, newUser.Role);
                if (user.Succeeded)
                {
                    return Ok("Registration successful.");
                }
                else
                {
                    return BadRequest(user.Errors);
                }
            
        }

        [HttpPost("registerJobSeeker")]
        public async Task<ActionResult> RegisterJobSeeker(JobSeekerRegistrationDto model)
        {
            User newUser = _mapper.Map<User>(model);
            JobSeeker jobSeeker = _mapper.Map<JobSeeker>(model);
            newUser.Role = RoleName.Employer;
            newUser.JobSeekerProfile = jobSeeker;
            var user = await _service.CreateUser(newUser, model.Password);
            await _roleService.AddRoleToUser(newUser, newUser.Role);
            if (user.Succeeded)
            {
                return Ok("Registration successful.");
            }
            else  return BadRequest(user.Errors);

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto model)
        {
            var user = await _service.Login(model.Username, model.Password, model.rememberme);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user.UserName);
        }
    }
}
