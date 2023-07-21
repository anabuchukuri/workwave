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

        public UserController( AuthService authService, RoleService roleService, IMapper mapper, IConfiguration configuration)
        {
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
                    return Ok("Employer user added");
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

        [HttpPost("registerUser")]
        public async Task<ActionResult> RegisterUser(UserRegistrationDto model, string Role)
        {
            User newUser = _mapper.Map<User>(model);
            var roleExist= await _roleService.RoleExistsAsync(Role);
            if(!roleExist) { return BadRequest("Role doesn't exist"); }
            newUser.Role = Role;
            var user = await _service.CreateUser(newUser, model.Password);
            await _roleService.AddRoleToUser(newUser, Role);
            if (user.Succeeded)
            {
                return Ok("user added");
            }
            else return BadRequest(user.Errors);

        }

        [HttpPost("changePassword")]
        public async Task<IdentityResult> ChangePassword(UserChangePasswordDto model)
        {
           
            return await _service.ChangePassword( model.Username, model.OldPassword, model.NewPassword);
        }

        [HttpPost("login")]
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
