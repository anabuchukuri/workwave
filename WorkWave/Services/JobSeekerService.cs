using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WorkWave.DbModels;
using WorkWave.DBModels;

namespace WorkWave.Services
{
    public class JobSeekerService
    {
        private readonly WorkwaveContext _context;

        public JobSeekerService(WorkwaveContext context)
        {
            _context = context;
        }
        public async Task<User> GetSeekerByUserId(int userId)
        {
             var i=await _context.User.Where(e=>e.Id==userId)
                .Include(u => u.JobSeekerProfile) 
                .FirstOrDefaultAsync();
            return i;
        }
    }
}
