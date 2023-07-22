using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos;
using WorkWave.Services.Abstracts;

namespace WorkWave.Services
{
    public class JobTypeService : IJobTypeService
    {
        private readonly WorkwaveContext _context;

        public JobTypeService(WorkwaveContext context)
        {
            _context = context;
        }
        public async Task<JobType> Add(JobType item)
        {  
            try
            {
                _context.JobType.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("An error occurred while adding the JobType. " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            
            var jobType = await _context.JobType.FindAsync(id);
            if (jobType == null)
            {
                throw new ApplicationException($"JobType with ID {id} not found.");
            }

            _context.JobType.Remove(jobType);
            await _context.SaveChangesAsync();
        }

        public async Task<JobType> Update(JobType item)
        {
          
            try
            {
                _context.JobType.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ApplicationException("The JobType could not be updated due to a concurrency conflict.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the JobType. " + ex.Message);
            }
        }

        public async Task<List<JobType>> GetAll()
        {
                return await _context.JobType
                    .ToListAsync();
        }

        public async Task<JobType> GetById(int id)
        {
            return await _context.JobType.FindAsync(id);
        }
    }
}
