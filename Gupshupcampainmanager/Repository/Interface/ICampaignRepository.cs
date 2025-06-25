using Gupshupcampaignmanager.Models.Common;
using Gupshupcampainmanager.Models;

namespace Gupshupcampainmanager.Repository.Interface
{
    public interface ICampaignRepository
    {
        Task<CampaignDetailsResponse> InsertCampainDetails(CampaignDetailsRequest model);
        Task<IEnumerable<CampaignDetails>> GetCampainDetails();
        Task<CampaignDetailsResponse> GetCampainDetailsById(int Id);
        Task<bool> DeletCampainDetailsById(int Id);
        Task<int> InsertCustomerAsync(CustomerReqeust request);
        Task<IEnumerable<CustomerViewModel>> ContextListAsync(CustomerReqeust reqeust);

        Task<bool> DeActiveCampaign(int Id, bool IsActive);
        Task<CampaignDetailsResponse> ActiveCampaign(bool IsActive);
        Task<bool> InsertOrUpdateSmsStatusAsync(SmsStatusRequest model);
    }
}
