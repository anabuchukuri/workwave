using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos;
using WorkWave.Services.Abstracts;

namespace WorkWave.Services
{
    public class JobCategoryService : IJobCategoryService
    {
        private readonly WorkwaveContext _context;

        public JobCategoryService(WorkwaveContext context)
        {
            _context = context;
        }
        public async Task<JobCategory> Add(JobCategory item)
        {  
            try
            {
                _context.JobCategory.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("An error occurred while adding the JobCategory. " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            var jobCategory = await _context.JobCategory.FindAsync(id);
            if (jobCategory == null)
            {
                throw new ApplicationException($"JobCategory with ID {id} not found.");
            }

            _context.JobCategory.Remove(jobCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<JobCategory> Update(JobCategory item)
        {
          
            try
            {
                _context.JobCategory.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ApplicationException("The JobCategory could not be updated due to a concurrency conflict.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the JobCategory. " + ex.Message);
            }
        }

        public async Task<List<JobCategory>> GetAll()
        {
                return await _context.JobCategory
                    .ToListAsync();
        }

        public async Task<JobCategory> GetById(int id)
        {
            return await _context.JobCategory.FindAsync(id);
        }
    }
}
