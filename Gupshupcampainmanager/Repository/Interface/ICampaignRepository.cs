using Gupshupcampainmanager.Models;

namespace Gupshupcampainmanager.Repository.Interface
{
    public interface ICampaignRepository
    {
        Task<CampaignDetailsResponse> InsertCampainDetails(CampaignDetailsRequest model);
    }
}
