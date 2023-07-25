using Microsoft.EntityFrameworkCore;
using WorkWave.DbModels;
using WorkWave.DBModels;

namespace WorkWave.Services
{
    public class EmployerService
    {
        private readonly WorkwaveContext _context;

        public EmployerService(WorkwaveContext context)
        {
            _context = context;
        }
        public async Task<User> GetEmployerByUserId(int userId)
        {
            var i = await _context.User.Where(e => e.Id == userId)
               .Include(u => u.EmployerProfile)
               .FirstOrDefaultAsync();
            return i;
        }

        public async Task<Employer> GetEmployerById(int id)
        {
            var i = await _context.Employer.Where(e => e.EmployerId == id)
               .FirstOrDefaultAsync();
            return i;
        }
    }
}
