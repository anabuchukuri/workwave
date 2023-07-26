using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkWave.Constants;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos;
using WorkWave.Services.Abstracts;

namespace WorkWave.Services
{
    public class JobOpeningService : IJobOpeningService
    {
        private readonly WorkwaveContext _context;

        public JobOpeningService(WorkwaveContext context)
        {
            _context = context;
        }
        public async Task<JobOpening> Add(JobOpening item)
        {  
            try
            {
                _context.JobOpening.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the JobOpening. " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            var jobOpening = await _context.JobOpening.FindAsync(id);
            if (jobOpening == null)
            {
                throw new ApplicationException($"JobOpening with ID {id} not found.");
            }

            var openingCategoriesToDelete =  _context.OpeningCategory
        .Where(oc => oc.JobOpeningId == id)
        .ToList();
            _context.OpeningCategory.RemoveRange(openingCategoriesToDelete);

            var applications = _context.JobApplication
        .Where(oc => oc.JobOpeningId == id)
        .ToList();
            _context.JobApplication.RemoveRange(applications);

            _context.JobOpening.Attach(jobOpening);
            _context.JobOpening.Remove(jobOpening);
            await _context.SaveChangesAsync();
        }

        public async Task<JobOpening> Update(JobOpening item)
        {
          
            try
            {
                _context.JobOpening.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ApplicationException("The JobOpening could not be updated due to a concurrency conflict.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the JobOpening. " + ex.Message);
            }
        }

        public async Task<List<JobOpening>> GetAll()
        {
                return await _context.JobOpening.Include(e => e.JobDetails).Include(e => e.Employer).ThenInclude(e => e.User)
                    .ToListAsync();
        }

        public async Task<int> checkAvailableOpenings(int jobOpeningId)
        {
            var jobApplications = await _context.JobApplication
        .Where(app => app.JobOpeningId == jobOpeningId && app.Status == Status.Accepted)
        .ToListAsync();

            var jobOpening = await this.GetById(jobOpeningId);

            var openings = jobOpening.JobDetails.NumberOfOpenings;
            if (openings!=null)
            {
                int openingsCount = (int)openings;
                int acceptedApplicationsCount = jobApplications.Count();
                return openingsCount - acceptedApplicationsCount;
            }
            return 1;
        }


        public async Task<JobOpening> GetById(int id)
        { 
            return await _context.JobOpening.Include(e => e.JobDetails).Include(e=>e.Employer).ThenInclude(e=>e.User).FirstOrDefaultAsync(jo => jo.JobOpeningId == id); ;
        }
    }
}
