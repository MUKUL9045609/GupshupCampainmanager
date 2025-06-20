using Dapper;
using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;

namespace Gupshupcampainmanager.Service
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly IGenericRepository<CampaignDetails> _repository;
        public CampaignRepository(IGenericRepository<CampaignDetails> repository)
        {
            _repository = repository;
        }

        public async Task<CampaignDetailsResponse> InsertCampainDetails(CampaignDetailsRequest model)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("Desciption", model.Desciption);
                parameters.Add("ImagePath", model.ImagePath);

                return await _repository.GetByValuesAsync<CampaignDetailsResponse>("sp_Insert_CampainDetails", parameters);
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public async Task<IEnumerable<CampaignDetails>> GetCampainDetails()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();  
                return await _repository.GetListByValuesAsync<CampaignDetails>("sp_Get_CampaignDetails", parameters);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
