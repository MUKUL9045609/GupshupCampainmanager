using Gupshupcampainmanager.Models;

namespace Gupshupcampainmanager.Repository.Interface
{
    public interface ICampaignRepository
    {
        Task<CampaignDetailsResponse> InsertCampainDetails(CampaignDetailsRequest model);
        Task<IEnumerable<CampaignDetails>> GetCampainDetails();
        Task<CampaignDetailsResponse> GetCampainDetailsById(int Id);
        Task<bool> DeletCampainDetailsById(int Id);

    }
}
