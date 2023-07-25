using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using WorkWave.Filters;
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

        public UserController( AuthService authService, RoleService roleService, IMapper mapper, IConfiguration configuration)
        {
            _service = authService;
            _roleService = roleService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("registerEmployer")]
        public async Task<ActionResult> RegisterEmployer(EmployerRegistrationDto model)
        {
            var roleExists =await _roleService.RoleExistsAsync("employer");
            if(!roleExists) BadRequest("Role doesnt exist");
            User newUser = _mapper.Map<User>(model);
                Employer employer = _mapper.Map<Employer>(model);
                newUser.Role = RoleName.Employer;
                newUser.EmployerProfile = employer;
                var user = await _service.CreateUser(newUser, model.Password);
                await _roleService.AddRoleToUser(newUser, newUser.Role);
                if (user.Succeeded)
                {
                    return Ok("Employer user added");
            }
                else
                {
                    return BadRequest(user.Errors);
                }
            
        }

        [AllowAnonymous]
        [HttpPost("registerJobSeeker")]
        public async Task<ActionResult> RegisterJobSeeker(JobSeekerRegistrationDto model)
        {
            var roleExists = await _roleService.RoleExistsAsync("jobseeker");
            if (!roleExists) BadRequest("Role doesnt exist");
            User newUser = _mapper.Map<User>(model);
            JobSeeker jobSeeker = _mapper.Map<JobSeeker>(model);
            newUser.Role = RoleName.JobSeeker;
            newUser.JobSeekerProfile = jobSeeker;
            var user = await _service.CreateUser(newUser, model.Password);
            await _roleService.AddRoleToUser(newUser, newUser.Role);
            if (user.Succeeded)
            {
                return Ok("user JobSeeker added"); 
            }
            else  return BadRequest(user.Errors);

        }

        [Authorize]
        [RoleFilter("admin")]
        [HttpPost("registerUser")]
        public async Task<ActionResult> RegisterAdmin(UserRegistrationDto model)
        {
            var roleExists = await _roleService.RoleExistsAsync("jobseeker");
            if (!roleExists) BadRequest("Role doesnt exist");
            User newUser = _mapper.Map<User>(model);
            newUser.Role = "admin";
            var user = await _service.CreateUser(newUser, model.Password);
            if (user.Succeeded)
            {
                await _roleService.AddRoleToUser(newUser, "admin");
                return Ok("user added");
            }
            else return BadRequest(user.Errors);

        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(UserChangePasswordDto model)
        {
            string userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName==model.Username)
            {
                var i=await _service.ChangePassword(model.Username, model.OldPassword, model.NewPassword);
                if(i.Succeeded) return Ok("password changed sucessfully");
                return BadRequest(i.Errors);
            }
            else return Unauthorized();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserLoginDto model)
        {
            var user = await _service.Login(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return GenerateToken(user);
        }

        private ObjectResult GenerateToken(User user)
        {
            var claims = new List<Claim>
                  {
                      new Claim(ClaimTypes.Name, user.UserName),
                      new Claim(ClaimTypes.NameIdentifier, ""+user.Id),
                      new Claim(ClaimTypes.Role, user.Role)
                  };
            var i = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value)),
                    SecurityAlgorithms.HmacSha256Signature),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }
    }
}
