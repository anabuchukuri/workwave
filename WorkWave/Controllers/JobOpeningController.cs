using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos.JobDetailDtos;
using WorkWave.Dtos.JobOpeningDtos;
using WorkWave.Filters;
using WorkWave.Services;
using WorkWave.Services.Abstracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOpeningController : ControllerBase
    {
        private readonly JobOpeningService _service;
        private readonly EmployerService _employerService;
        private readonly JobCategoryService _jobCategoryService;
        private readonly IMapper _mapper;

        /*private readonly WorkwaveContext _context;*/

        public JobOpeningController(JobOpeningService jobOpeningService, EmployerService employerService, JobCategoryService jobCategoryService, IMapper mapper )
        {
            _service = jobOpeningService;
            _employerService = employerService;
            _jobCategoryService = jobCategoryService;
            _mapper = mapper;
        }

        // GET: api/<JobOpeningController>
       
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<JobOpeningDto>>> GetAll()
        {
            var jobOpenings= await _service.GetAll();
            if (jobOpenings.Count == 0)
            {
                return NoContent(); 
            }
            var jobOpeningDtos = _mapper.Map<List<JobOpeningDto>>(jobOpenings);
            return Ok(jobOpeningDtos);
        }

        // GET api/<JobOpeningController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<JobOpeningResponseDto>> Get(int id)
        {
            var jobOpening = await _service.GetById(id);
            if (jobOpening == null)
            {
                return NotFound();
            }
            var JobOpeningResponseDto = new JobOpeningResponseDto()
            {
                JobOpeningId = jobOpening.JobOpeningId,
                Title = jobOpening.Title,
                Description = jobOpening.Description,
                Location = jobOpening.Location,
                Salary = jobOpening.Salary,
                IsActive = jobOpening.IsActive,
                CreationDate = jobOpening.CreationDate,
                EmployerId = jobOpening.EmployerId,
                EmployerCompanyName = jobOpening.Employer.CompanyName,
                EmployercontactNumber = jobOpening.Employer.ContactNumber,
                AuthorUserId = jobOpening.Employer.User.Id,
                JobDetailsId = jobOpening.JobDetails.JobDetailsId,
                EmploymentType = jobOpening.JobDetails.EmploymentType,
                ApplicationDeadline = jobOpening.JobDetails.ApplicationDeadline,
                RequiredExperience = jobOpening.JobDetails.RequiredExperience,
                Qualifications = jobOpening.JobDetails.Qualifications,
                Responsibilities = jobOpening.JobDetails.Responsibilities,
                CompanyCulture = jobOpening.JobDetails.CompanyCulture,
                ApplicationInstructions = jobOpening.JobDetails.ApplicationInstructions,
                NumberOfOpenings = jobOpening.JobDetails.NumberOfOpenings,
                IsFullTime = jobOpening.JobDetails.IsFullTime,
                IsRemote = jobOpening.JobDetails.IsRemote,
            };
            return Ok(JobOpeningResponseDto);
        }

        // POST api/<JobOpeningController>
        [Authorize]
        [HttpPost]
        [RoleFilter("Employer")]
        public async Task<ActionResult<JobOpeningResponseDto>> Post(JobOpeningAddDto jobOpeningAddDto)
        {
            try
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(userId == null) { return Unauthorized(); }
                var user = await _employerService.GetEmployerByUserId(int.Parse(userId));
                // Map the DTO to the entity model
                var jobOpening = _mapper.Map<JobOpening>(jobOpeningAddDto);
                jobOpening.CreationDate = DateTime.Now;
                jobOpening.Employer = user.EmployerProfile;
                if (jobOpeningAddDto.JobCategoryList != null)
                {
                    jobOpening.OpeningCategories = jobOpeningAddDto.JobCategoryList
                    .Select(e => new OpeningCategory
                    {
                        JobCategoryId = e
                    })
                    .ToList();
                }
                var jobDetails = new JobDetails()
                {
                    EmploymentType = jobOpeningAddDto.EmploymentType,
                    ApplicationDeadline = jobOpeningAddDto.ApplicationDeadline,
                    RequiredExperience = jobOpeningAddDto.RequiredExperience,
                    Qualifications = jobOpeningAddDto.Qualifications,
                    Responsibilities = jobOpeningAddDto.Responsibilities,
                    CompanyCulture = jobOpeningAddDto.CompanyCulture,
                    ApplicationInstructions = jobOpeningAddDto.ApplicationInstructions,
                    NumberOfOpenings = jobOpeningAddDto.NumberOfOpenings,
                    IsFullTime = jobOpeningAddDto.IsFullTime,
                    IsRemote = jobOpeningAddDto.IsRemote
                };
                jobOpening.JobDetails = jobDetails;
                jobDetails.JobOpening = jobOpening;
                var createdJobOpening = await _service.Add(jobOpening);

                var JobOpeningResponseDto = new JobOpeningResponseDto()
                {
                    JobOpeningId = createdJobOpening.JobOpeningId,
                    Title = createdJobOpening.Title,
                    Description = createdJobOpening.Description,
                    Location = createdJobOpening.Location,
                    Salary = createdJobOpening.Salary,
                    IsActive = createdJobOpening.IsActive,
                    CreationDate = createdJobOpening.CreationDate,
                    EmployerId = createdJobOpening.EmployerId,
                    EmployerCompanyName = createdJobOpening.Employer.CompanyName,
                    EmployercontactNumber = createdJobOpening.Employer.ContactNumber,
                    AuthorUserId = createdJobOpening.Employer.User.Id,
                    JobDetailsId = createdJobOpening.JobDetails.JobDetailsId,
                    EmploymentType = createdJobOpening.JobDetails.EmploymentType,
                    ApplicationDeadline = createdJobOpening.JobDetails.ApplicationDeadline,
                    RequiredExperience = createdJobOpening.JobDetails.RequiredExperience,
                    Qualifications = createdJobOpening.JobDetails.Qualifications,
                    Responsibilities = createdJobOpening.JobDetails.Responsibilities,
                    CompanyCulture = createdJobOpening.JobDetails.CompanyCulture,
                    ApplicationInstructions = createdJobOpening.JobDetails.ApplicationInstructions,
                    NumberOfOpenings = createdJobOpening.JobDetails.NumberOfOpenings,
                    IsFullTime = createdJobOpening.JobDetails.IsFullTime,
                    IsRemote = createdJobOpening.JobDetails.IsRemote,
                };
                return Ok(JobOpeningResponseDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<JobOpeningController>/5
        [Authorize]
        [RoleFilter("Employer")]
        [HttpPut("{id}")]
        public async Task<ActionResult<JobOpeningDto>> Put(int id, [FromBody] JobOpeningUpdateDto JobOpeningDto)
        {
            try
            {
                var existingJobOpening = await _service.GetById(id);
                if (existingJobOpening == null)
                {
                    return NotFound();
                }
                 _mapper.Map(JobOpeningDto, existingJobOpening);
            
                var updatedJobOpening = await _service.Update(existingJobOpening);
                var updatedJobOpeningDto = _mapper.Map<JobOpeningDto>(updatedJobOpening);
                return Ok(updatedJobOpeningDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<JobOpeningController>/5
        [HttpDelete("{id}")]
        [Authorize]
        [RoleFilter("Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
