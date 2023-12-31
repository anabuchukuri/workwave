﻿using Microsoft.EntityFrameworkCore;
using WorkWave.Constants;
using WorkWave.DbModels;
using WorkWave.DBModels;
using WorkWave.Dtos.JobApplicationDtos;
using WorkWave.Services.Abstracts;

namespace WorkWave.Services
{
    public class JobApplicationService: IJobApplicationService
    {
        private readonly WorkwaveContext _context;

        public JobApplicationService(WorkwaveContext context)
        {
            _context = context;
        }

        public async Task<JobApplication> Add(JobApplication item)
        {
            try
            {
                _context.JobApplication.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("An error occurred while adding the JobApplication. " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            var JobApplication = await _context.JobApplication.FindAsync(id);
            if (JobApplication == null)
            {
                throw new ApplicationException($"JobApplication with ID {id} not found.");
            }

            _context.JobApplication.Remove(JobApplication);
            await _context.SaveChangesAsync();
        }

        public async Task<JobApplication> Update(JobApplication item)
        {

            try
            {
                _context.JobApplication.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ApplicationException("The JobApplication could not be updated due to a concurrency conflict.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the JobApplication. " + ex.Message);
            }
        }

        public async Task<List<JobApplication>> GetAll()
        {
            return await _context.JobApplication
                .ToListAsync();
        }

        public async Task<JobApplicationGetDto> GetByIdWithDetails(int id)
        {
            return await _context.JobApplication
                .Where(e => e.ApplicationId == id)
                .Include(e => e.JobSeeker)
                .Include(e => e.JobOpening)
                .ThenInclude(jobOpening => jobOpening.Employer)
                .Select(e => new JobApplicationGetDto
            {
                ApplicationId = e.ApplicationId,
                ApplicationDate = e.ApplicationDate,
                CoverLetter = e.CoverLetter,
                References = e.References,
                JobSpecificCV = e.JobSpecificCV,
                JobSeekerId = e.JobSeekerId,
                JobSeekerName = e.JobSeeker.FirstName + " " + e.JobSeeker.LastName,
                JobOpeningId = e.JobOpeningId,
                JobOpeningCompanyName = e.JobOpening.Employer.CompanyName
            }).FirstOrDefaultAsync();
        }

        public async Task<JobApplication> GetById(int id)
        {
            return await _context.JobApplication.FirstOrDefaultAsync(jo => jo.ApplicationId == id); ;
        }

        public async Task<List<JobApplication>> GetApplicationsForEmployer(int id, Status? status)
        {
            var employer = await _context.Employer
           .Include(e => e.JobOpenings)
           .ThenInclude(jo => jo.JobDetails)
           .Include(e => e.JobOpenings)
           .ThenInclude(jo => jo.JobApplications)
           .FirstOrDefaultAsync(e => e.EmployerId == id);
            return employer.JobOpenings
                .SelectMany(jo => jo.JobApplications)
                .Where(app => status!=null? app.Status == status:true)
                .ToList(); 
        }

        public async Task<JobApplication> ChangeApplicationStatus(int id, Status status)
        {

            var jobApplication = await _context.JobApplication
         .FirstOrDefaultAsync(app => app.ApplicationId == id);

            if (jobApplication == null)
            {
                // Return null or handle the case when the job application is not found.
                return null;
            }

            // Update the status of the job application
            jobApplication.Status = status;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return jobApplication;
        }
    }
}
