using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly IMapper _mapper;

        /*private readonly WorkwaveContext _context;*/

        public JobApplicationController(JobApplicationService jobApplicationService, JobSeekerService JobSeekerService, IMapper mapper)
        {
            _seekerService = JobSeekerService;
            _service = jobApplicationService;
            _mapper = mapper;
        }

        // GET: api/<JobOpeningController>

        [HttpGet]
        public async Task<ActionResult<List<JobApplicationAddDto>>> GetAll()
        {
            var jobApplication = await _service.GetAll();
            if (jobApplication.Count == 0)
            {
                return NoContent();
            }
            var jobApplicationDtos = _mapper.Map<List<JobApplicationDto>>(jobApplication);
            return Ok(jobApplicationDtos);
        }

        // GET api/<JobOpeningController>/5
        [HttpGet("{id}")]

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

        // POST api/<JobOpeningController>
        [HttpPost]
        [RoleFilter("jobseeker")]
        public async Task<ActionResult<JobApplication>> Post(JobApplicationAddDto jobApplicationAddDto)
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
                return Ok(createdJobApplication);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<JobOpeningController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<JobApplicationDto>> Put(int id, [FromBody] JobApplicationDto JobApplicationDto)
        {
            var existingJobApplication = await _service.GetById(id);
            if (existingJobApplication == null)
            {
                return NotFound();
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

        // DELETE api/<JobOpeningController>/5
        [HttpDelete("{id}")]
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

