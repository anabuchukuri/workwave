using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos.JobCategoryDtos;
using WorkWave.Dtos.JobOpeningDtos;
using WorkWave.Services;
using WorkWave.Services.Abstracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCategoryController : ControllerBase
    {
        private readonly JobCategoryService _service;
        private readonly IMapper _mapper;

        /*private readonly WorkwaveContext _context;*/

        public JobCategoryController(JobCategoryService jobCategoryService, IMapper mapper )
        {
            _service = jobCategoryService;
            _mapper = mapper;
        }

        // GET: api/<JobCategoryController>
        [HttpGet]
        public async Task<ActionResult<List<JobCategoryDto>>> GetAll()
        {
            var jobCategories = await _service.GetAll();
            if (jobCategories.Count == 0)
            {
                return NoContent(); 
            }
            var JobCategoryDtos = _mapper.Map<List<JobCategoryDto>>(jobCategories);
            return Ok(JobCategoryDtos);
        }

        // GET api/<JobCategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobCategoryDto>> Get(int id)
        {
            var jobCategory = await _service.GetById(id);
            if (jobCategory == null)
            {
                return NotFound();
            }
            var jobCategoryDto = _mapper.Map<JobCategoryDto>(jobCategory);
            return Ok(jobCategoryDto);
        }

        // POST api/<JobCategoryController>
        [HttpPost]
        public async Task<ActionResult<JobCategoryDto>> Post(JobCategoryAddDto jobCategoryAddDto)
        {
            // Map the DTO to the entity model
            var jobCategory = _mapper.Map<JobCategory>(jobCategoryAddDto);
            try
            {
                var createdJobCategory = await _service.Add(jobCategory);
                var createdJobCategoryDto = _mapper.Map<JobCategoryDto>(createdJobCategory);
                return Ok(createdJobCategoryDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

            // PUT api/<JobCategoryController>/5
            [HttpPut("{id}")]
        public async Task<ActionResult<JobCategoryDto>> Put(int id, [FromBody] JobCategoryAddDto JobCategoryDto)
        {
            var existingJobCategory = await _service.GetById(id);
            if (existingJobCategory == null)
            {
                return NotFound();
            }
             _mapper.Map(JobCategoryDto, existingJobCategory);
            try
            {
                var updatedJobCategory = await _service.Update(existingJobCategory);
                var updatedJobCategoryDto = _mapper.Map<JobCategoryDto>(updatedJobCategory);
                return Ok(updatedJobCategoryDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<JobCategoryController>/5
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
