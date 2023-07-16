using WorkWave.DBModels;

namespace WorkWave.Services.Abstracts
{
    public interface IJobOpeningService
    {

        Task<List<JobOpening>> GetAll();

        Task<JobOpening> GetById(int id);

        Task<JobOpening> Add(JobOpening item);

        Task<JobOpening> Update(JobOpening item);

        Task Delete(int id);

        //TODO
        /*Task<List<JobOpening>> GetMostPopularOffers();*/

        /*Task<List<JobOpening>> GetOffersContainingPhrase(string phrase);*/

       /* Task<bool> CanUserEditOffer(string userId, string offerId);*/

        /*Task<bool> IncreaseOfferViews(JobOffer offer);*/
    }
}
