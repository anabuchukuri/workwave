using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos.JobOpeningDtos;
using WorkWave.Dtos.JobTypeDtos;
using WorkWave.Filters;
using WorkWave.Services;
using WorkWave.Services.Abstracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTypeController : ControllerBase
    {
        private readonly JobTypeService _service;
        private readonly IMapper _mapper;

        /*private readonly WorkwaveContext _context;*/

        public JobTypeController(JobTypeService jobTypeService, IMapper mapper )
        {
            _service = jobTypeService;
            _mapper = mapper;
        }

        // GET: api/<JobTypeController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<JobTypeDto>>> GetAll()
        {
            var jobTypes = await _service.GetAll();
            if (jobTypes.Count == 0)
            {
                return NoContent(); 
            }
            var JobTypeDtos = _mapper.Map<List<JobTypeDto>>(jobTypes);
            return Ok(JobTypeDtos);
        }

        // GET api/<JobTypeController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTypeDto>> Get(int id)
        {
            var jobType = await _service.GetById(id);
            if (jobType == null)
            {
                return NotFound();
            }
            var jobTypeDto = _mapper.Map<JobTypeDto>(jobType);
            return Ok(jobTypeDto);
        }

        // POST api/<JobTypeController>
        [Authorize]
        [RoleFilter("admin")]
        [HttpPost]
        public async Task<ActionResult<JobTypeDto>> Post(JobTypeAddDto jobTypeAddDto)
        {
            // Map the DTO to the entity model
            var jobType = _mapper.Map<JobType>(jobTypeAddDto);
            try
            {
                var createdJobType = await _service.Add(jobType);
                var createdJobTypeDto = _mapper.Map<JobTypeDto>(createdJobType);
                return Ok(createdJobTypeDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<JobTypeController>/5
        [Authorize]
        [RoleFilter("admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<JobTypeDto>> Put(int id, [FromBody] JobTypeAddDto JobTypeDto)
        {
            var existingJobType = await _service.GetById(id);
            if (existingJobType == null)
            {
                return NotFound();
            }
             _mapper.Map(JobTypeDto, existingJobType);
            try
            {
                var updatedJobType = await _service.Update(existingJobType);
                var updatedJobTypeDto = _mapper.Map<JobTypeDto>(updatedJobType);
                return Ok(updatedJobTypeDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<JobTypeController>/5
        [Authorize]
        [RoleFilter("admin")]
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
