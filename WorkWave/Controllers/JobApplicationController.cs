using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using WorkWave.Constants;
using WorkWave.DBModels;
using WorkWave.Dtos.JobApplicationDtos;
using WorkWave.Dtos.JobOpeningDtos;
using WorkWave.Filters;
using WorkWave.Services;
using WorkWave.Services.Abstracts;

namespace WorkWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {

        private readonly JobApplicationService _service;
        private readonly JobSeekerService _seekerService;
        private readonly EmployerService _employerService;
        private readonly JobOpeningService _jobOpeningService;
        private readonly IMapper _mapper;


        public JobApplicationController(JobApplicationService jobApplicationService, JobOpeningService jobOpeningService, JobSeekerService JobSeekerService, EmployerService employerService, IMapper mapper)
        {
            _seekerService = JobSeekerService;
            _service = jobApplicationService;
            _employerService = employerService;
            _jobOpeningService = jobOpeningService;
            _mapper = mapper;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<JobApplicationDto>>> GetAll()
        {
            var jobApplication = await _service.GetAll();
            if (jobApplication.Count == 0)
            {
                return NoContent();
            }
            var jobApplicationDtos = _mapper.Map<List<JobApplicationDto>>(jobApplication);
            return Ok(jobApplicationDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<JobApplicationDto>> Get(int id)
        {
            var jobApplication = await _service.GetByIdWithDetails(id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            var jobApplicationDto = _mapper.Map<JobApplicationDto>(jobApplication);
            return Ok(jobApplicationDto);
        }

        [HttpPost]
        [Authorize]
        [RoleFilter("jobseeker")]
        public async Task<ActionResult<JobApplicationDto>> Post(JobApplicationAddDto jobApplicationAddDto)
        {
            try
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _seekerService.GetSeekerByUserId(int.Parse(userId));
                // Map the DTO to the entity model
                var jobApplication = _mapper.Map<JobApplication>(jobApplicationAddDto);
                jobApplication.JobSeeker = user.JobSeekerProfile;
                jobApplication.ApplicationDate = DateTime.Now;

                var createdJobApplication = await _service.Add(jobApplication);
                var jobApplicationDto = _mapper.Map<JobApplicationDto>(createdJobApplication);
                return Ok(jobApplicationDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [RoleFilter("jobseeker")]
        public async Task<ActionResult<JobApplicationDto>> Put(int id, [FromBody] JobApplicationAddDto JobApplicationDto)
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _seekerService.GetSeekerByUserId(int.Parse(userId));
            
            var existingJobApplication = await _service.GetById(id);
            if (existingJobApplication == null)
            {
                return NotFound();
            }
            if (user.JobSeekerId != existingJobApplication.JobSeekerId)
            {
                return Unauthorized();
            }
            
            _mapper.Map(JobApplicationDto, existingJobApplication);
            try
            {
                var updatedJobOpening = await _service.Update(existingJobApplication);
                var updatedJobOpeningDto = _mapper.Map<JobApplicationDto>(updatedJobOpening);
                return Ok(updatedJobOpeningDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        [RoleFilter("jobseeker")]
        /*TODO*/
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _seekerService.GetSeekerByUserId(int.Parse(userId));

                var existingJobApplication = await _service.GetById(id);
                if (existingJobApplication == null)
                {
                    return NotFound();
                }
                if (user.JobSeekerId != existingJobApplication.JobSeekerId)
                {
                    return Unauthorized();
                }

                await _service.Delete(id);
                return Ok("job application deleted");
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("GetApplicationsForEmployer")]
        [Authorize]
        [RoleFilter("Employer")]
        public async Task<IActionResult> GetApplicationsForEmployer( Status status)
        {
            try
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return NotFound("Employer not fount");
                var user = await _employerService.GetEmployerByUserId(int.Parse(userId));
                if (user == null || user.EmployerId == null) return NotFound("Employer not fount");
                var employer = user.EmployerProfile;
                // Check if the employer exists
                if (employer == null)
                {
                    return NotFound("Employer not found.");
                }

                // Retrieve job applications for the specific employer and with the given status
                var jobApplications = await _service.GetApplicationsForEmployer((int)user.EmployerId, status);
                var dto = _mapper.Map<List<JobApplicationDto>>(jobApplications);
                return Ok(dto);
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("ChangeStatus")]
        [Authorize]
        [RoleFilter("Employer")]
        public async Task<IActionResult> ChangeApplicationStatus(int id, Status status)
        {
            try
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return NotFound("Employer not fount");
                var user= await _employerService.GetEmployerByUserId(int.Parse(userId));
                if(user==null || user.EmployerId==null) return NotFound("Employer not fount");
                var jobApplications = await _service.GetApplicationsForEmployer((int)user.EmployerId, null);
                JobApplication application = jobApplications.Find(job => job.ApplicationId == id);
                if (application == null) return NotFound("user's application not found");
                if (status == Status.Accepted)
                {
                    int count = await _jobOpeningService.checkAvailableOpenings(application.JobOpeningId);
                    if (count == 0) return NotFound("maximum number of applicants has been reached");
                }
                var result = await _service.ChangeApplicationStatus(id, status);
                var dto = _mapper.Map<JobApplicationDto>(result);
                return Ok(dto);
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

