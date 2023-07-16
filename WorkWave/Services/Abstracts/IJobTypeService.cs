using WorkWave.DBModels;

namespace WorkWave.Services.Abstracts
{
    public interface IJobTypeService
    {

        Task<List<JobType>> GetAll();

        Task<JobType> GetById(int id);

        Task<JobType> Add(JobType item);

        Task<JobType> Update(JobType item);

        Task Delete(int id);
    }
}
