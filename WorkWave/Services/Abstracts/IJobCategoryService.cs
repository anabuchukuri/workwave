using WorkWave.DBModels;

namespace WorkWave.Services.Abstracts
{
    public interface IJobCategoryService
    {

        Task<List<JobCategory>> GetAll();

        Task<JobCategory> GetById(int id);

        Task<JobCategory> Add(JobCategory item);

        Task<JobCategory> Update(JobCategory item);

        Task Delete(int id);
    }
}
