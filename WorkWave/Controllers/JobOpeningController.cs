using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos.JobOpeningDtos;
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
        private readonly IMapper _mapper;

        /*private readonly WorkwaveContext _context;*/

        public JobOpeningController(JobOpeningService jobOpeningService, IMapper mapper )
        {
            _service = jobOpeningService;
            _mapper = mapper;
        }

        // GET: api/<JobOpeningController>
        [Authorize]
        [HttpGet]
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
        public async Task<ActionResult<JobOpeningDto>> Get(int id)
        {
            var jobOpening = await _service.GetById(id);
            if (jobOpening == null)
            {
                return NotFound();
            }
            var jobOpeningDto = _mapper.Map<JobOpeningDto>(jobOpening);
            return Ok(jobOpeningDto);
        }

        // POST api/<JobOpeningController>
        [HttpPost]
        public async Task<ActionResult<JobOpeningDto>> Post(JobOpeningAddDto jobOpeningAddDto)
        {
            // Map the DTO to the entity model
            var jobOpening = _mapper.Map<JobOpening>(jobOpeningAddDto);
            jobOpening.CreationDate = DateTime.Now;
            try
            {
                var createdJobOpening = await _service.Add(jobOpening);
                var createdJobOpeningDto = _mapper.Map<JobOpeningDto>(createdJobOpening);
                return Ok(createdJobOpeningDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

            // PUT api/<JobOpeningController>/5
            [HttpPut("{id}")]
        public async Task<ActionResult<JobOpeningDto>> Put(int id, [FromBody] JobOpeningAddDto JobOpeningDto)
        {
            var existingJobOpening = await _service.GetById(id);
            if (existingJobOpening == null)
            {
                return NotFound();
            }
             _mapper.Map(JobOpeningDto, existingJobOpening);
            try
            {
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
