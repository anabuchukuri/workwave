using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IMapper _mapper; 
        private readonly IConfiguration _configuration;
        
        /*private readonly WorkwaveContext _context;*/

        public UserController(AuthService authService, IMapper mapper, IConfiguration configuration)
        {
            _service = authService;
            _mapper = mapper;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegistrationDto model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _service.Register(user, model.Password);
            
            if (result.Succeeded)
            {
                return Ok("Registration successful.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
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
